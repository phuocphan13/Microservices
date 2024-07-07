namespace ApiClient.Basket.Events;

public class BaseMessage
{
    public string UserId { get; set; } = null!;
    
    public string UserName { get; set; } = null!;
}