using ApiClient.Basket.Models;

namespace ApiClient.Basket.Events.CheckoutEvents;

public class BasketCheckoutMessage 
    // : BaseMessage
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    
    public DateTime Timestamp { get; set; }

    // public List<BasketItemSummary> Items { get; set; } = new();
}