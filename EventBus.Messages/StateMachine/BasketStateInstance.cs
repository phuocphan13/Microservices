using MassTransit;

namespace EventBus.Messages.StateMachine;

public class BasketStateInstance : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    
    public string UserId { get; set; } = null!;

    public string CurrentState { get; set; } = null!;
    
    public int Version { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime Timestamp { get; set; }
}