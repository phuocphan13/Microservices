using EventBus.Messages.Entities;
using MassTransit;

namespace EventBus.Messages.StateMachine.Logging;

public class LogStateDefinition : SagaDefinition<LogState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<LogState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));

        endpointConfigurator.UseEntityFrameworkOutbox<OutboxMessageDbContext>(context);
    }
}