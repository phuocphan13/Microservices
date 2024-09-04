using ApiClient.Basket.Models;
using MediatR;

namespace Ordering.Application.Features.Commands.CheckoutOrder;

public class CheckoutOrderCommand : IRequest<bool>
{
    public string UserId { get; set; } = null!;
    
    public string UserName { get; set; } = null!;

    public string BasketKey { get; set; } = null!;
    
    public decimal TotalPrice { get; set; }
    

    public DateTime Timestamp { get; set; }
    
    public string MemberId { get; set; } = null!;

    public string EventId { get; set; } = null!;
    

    public ICollection<BasketItemSummary> Items { get; set; } = new List<BasketItemSummary>();

    public ICollection<DiscountItemSummary> DiscountItems { get; set; } = new List<DiscountItemSummary>();

    public ICollection<CouponItemSummary> CouponItems { get; set; } = new List<CouponItemSummary>();
}
