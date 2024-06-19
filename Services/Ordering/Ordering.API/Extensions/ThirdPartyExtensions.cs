using EventBus.Messages.Extensions;
using EventBus.Messages.StateMachine;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.EventBusConsumer;

namespace Ordering.API.Extensions;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParties(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<BasketCheckoutConsumer>();
            config.AddSagaStateMachine<BasketStateMachine, BasketStateInstance>()
                .RedisRepository(r => { r.DatabaseConfiguration(configuration["ConnectionStrings:SagaConnectionString"]); });

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddHostedService<MassTransitConsoleHostedService>();
        
        // builder.Services.AddScoped<BasketCheckoutConsumer>();
        
        return services;
    }
}