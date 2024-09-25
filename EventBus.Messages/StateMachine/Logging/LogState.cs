using MassTransit;

namespace EventBus.Messages.StateMachine.Logging;

public class LogState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }

    public string Text { get; set; } = null!;
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public int Type { get; set; }
    
    public string CurrentState { get; set; } = null!;
}