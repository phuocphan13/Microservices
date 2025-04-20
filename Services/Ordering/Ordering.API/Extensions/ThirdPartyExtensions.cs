using EventBus.Messages;
using EventBus.Messages.Entities;
using EventBus.Messages.Extensions;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.EventBusConsumer;
using Ordering.Application.WorkerServices;

namespace Ordering.API.Extensions;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParties(this IServiceCollection services, IConfiguration configuration)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddHostedService<MassTransitConsoleHostedService>();

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
            x.AddConsumer<BasketCheckoutConsumer>();
            x.AddConsumer<FailureOrderConsumer>();
        }, sagaAction: x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OutboxMessageDbContext>();
                    r.UseSqlServer();
                });
        });
        
        //Workers
        services.AddHostedService<OrderWokerJobService>();
        
        return services;
    }
}