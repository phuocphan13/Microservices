using ApiClient.Logging.Events;
using MassTransit;

namespace EventBus.Messages.StateMachine.Logging;

public class LogStateMachine : MassTransitStateMachine<LogState>
{
    public Event<SaveLogsMessage> SaveLogsEvent { get; private set; } = null!;

    public State Sent { get; private set; } = null!;
    public State Stored { get; private set; } = null!;
    
    public LogStateMachine()
    {
        Event(() => SaveLogsEvent, x => x.CorrelateById(context => Guid.Parse(context.Message.CorrelationId)));

        InstanceState(x => x.CurrentState);

        Initially(
            When(SaveLogsEvent)
                .Then(context =>
                {
                })
                .TransitionTo(Sent));
    }
}