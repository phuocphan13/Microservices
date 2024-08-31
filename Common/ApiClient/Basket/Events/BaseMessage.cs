namespace ApiClient.Basket.Events;

public class BaseMessage
{
    public string MemberId { get; set; } = Guid.NewGuid().ToString();
    
    public string EventId { get; set; } = Guid.NewGuid().ToString();

    public ProcessEnum Proccess { get; set; }

    public string Description { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
}