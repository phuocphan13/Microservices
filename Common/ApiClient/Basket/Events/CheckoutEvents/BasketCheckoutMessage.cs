using System.ComponentModel;
using ApiClient.Basket.Models;
using ApiClient.Common.MessageQueue;

namespace ApiClient.Basket.Events.CheckoutEvents;

[Description(EventBusConstants.OrderProccess.Checkout)]
public class BasketCheckoutMessage : BaseMessage
{
    public string BasketKey { get; set; } = null!;
    public decimal TotalPrice { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ICollection<BasketItemSummary> Items { get; set; } = new List<BasketItemSummary>();
    
    public ICollection<DiscountItemSummary> DiscountItems { get; set; } = new List<DiscountItemSummary>();
    
    public ICollection<CouponItemSummary> CouponItems { get; set; } = new List<CouponItemSummary>();
}