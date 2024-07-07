namespace ApiClient.Basket.Events;

public class BaseMessage
{
    public string MemberId { get; set; } = Guid.NewGuid().ToString();
    
    public string EventId { get; set; } = Guid.NewGuid().ToString();
}