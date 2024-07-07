namespace ApiClient.Basket.Models;

public class BasketDetail : BasketSummary
{
    public ICollection<DiscountItemSummary> DiscountItems { get; set; } = new List<DiscountItemSummary>();
    
    public ICollection<CouponItemSummary> CouponItems { get; set; } = new List<CouponItemSummary>();
}