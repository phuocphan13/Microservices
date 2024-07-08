using EventBus.Messages.Entities;
using MassTransit;

namespace EventBus.Messages.StateMachine.Basket;

public class BasketStateDefinition : SagaDefinition<BasketState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<BasketState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<MessageDbContext>(context);
    }
}