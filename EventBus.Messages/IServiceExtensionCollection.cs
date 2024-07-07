using System.Reflection;
using EventBus.Messages.Entities;
using EventBus.Messages.Services;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Database.Hosts;

namespace EventBus.Messages;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IQueueService, QueueService>();
        services.AddScoped<IPublishService, PublishService>();
        
        return services;
    }

    public static IServiceCollection AddMessageOutboxCosumer(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? busAction = default)
    {
        services.AddDbContext<MessageDbContext>(x =>
        {
            var connectionString = configuration["EventBusSettings:ConnectionString"];

            x.UseSqlServer(connectionString, options =>
            {
                options.MinBatchSize(1);
            });
        });
        
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<MessageDbContext>(o =>
            {
                o.UseSqlServer();

                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });

            x.SetKebabCaseEndpointNameFormatter();

            // Add consumers
            busAction?.Invoke(x);
            
            x.AddSagaStateMachine<BasketStateMachine, BasketState, BasketStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<MessageDbContext>();
                    r.UseSqlServer();
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);
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
    
    private static IServiceCollection AddMessageDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MessageDbContext>(x =>
        {
            var connectionString = configuration["EventBusSettings:ConnectionString"];

            x.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                options.MigrationsHistoryTable($"__{nameof(MessageDbContext)}");

                options.EnableRetryOnFailure(5);
                options.MinBatchSize(1);
            });
        });

        services.AddHostedService<RecreateDatabaseHostedService<MessageDbContext>>();

        return services;
    }
    
    private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<MessageDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);

                o.UseSqlServer();
                o.DisableInboxCleanupService();
                o.UseBusOutbox();
            });
            
            x.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);
                cfg.AutoStart = true;
            });
        });

        return services;
    }
}