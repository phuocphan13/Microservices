namespace EventBus.Messages.Messages;

public class BaseMessage
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
}