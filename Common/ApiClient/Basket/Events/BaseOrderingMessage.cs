namespace ApiClient.Basket.Events;

public class BaseOrderingMessage : BaseMessage
{
    public ProcessEnum Proccess { get; set; }

    public string Description { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
}