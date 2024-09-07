using System.Reflection;
using EventBus.Messages.Entities;
using EventBus.Messages.Services;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Database.Hosts;
using Platform.Extensions;

namespace EventBus.Messages;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IQueueService, QueueService>();
        services.AddScoped<IPublishService, PublishService>();
        services.AddScoped<IBasketMessageService, BasketMessageService>();
        
        return services;
    }

    public static IServiceCollection AddMessageOutboxCosumer(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? busAction = default)
    {
        services.AddMessageDbContext(configuration);
        
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<OrderMessageDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                o.UseSqlServer();
                o.DisableInboxCleanupService();
                o.UseBusOutbox();
            });

            x.SetKebabCaseEndpointNameFormatter();
            
            // Add consumers
            busAction?.Invoke(x);
            
            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OrderMessageDbContext>();
                    r.UseSqlServer();
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConfigurationValue("EventBusSettings:HostAddress"));
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddMessageOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageService, MessageService>();
        
        services
            .AddMessageDbContext(configuration)
            .AddMassTransit(configuration);

        return services;
    }

    private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<OrderMessageDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);

                o.UseSqlServer();
                o.DisableInboxCleanupService();
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(configuration.GetConfigurationValue("EventBusSettings:HostAddress"));
                cfg.AutoStart = true;
            });
        });

        return services;
    }
    
    private static IServiceCollection AddMessageDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderMessageDbContext>(x =>
        {
            var connectionString = configuration.GetConfigurationValue("EventBusSettings:ConnectionString");

            x.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                options.MigrationsHistoryTable($"__{nameof(OrderMessageDbContext)}");

                options.EnableRetryOnFailure(5);
                options.MinBatchSize(1);
            });
        });

        services.AddHostedService<RecreateDatabaseHostedService<OrderMessageDbContext>>();

        return services;
    }
}