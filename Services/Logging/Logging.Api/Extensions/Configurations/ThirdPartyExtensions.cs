using EventBus.Messages;
using EventBus.Messages.Entities;
using EventBus.Messages.Extensions;
using EventBus.Messages.StateMachine.Basket;
using Logging.Consumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Platform;

namespace Logging.Extensions.Configurations;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParties(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddHostedService<MassTransitConsoleHostedService>();

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
            x.AddConsumer<SaveLogConsumer>();
        }, sagaAction: x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OutboxMessageDbContext>();
                    r.UseSqlServer();
                });
        });

        services.AddElasticServices();
        
        return services;
    }
}