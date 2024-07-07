namespace ApiClient.Basket.Models;

public class BaseBasketRequestBody
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public List<BasketItemSummary> Items { get; set; } = new();

    public IEnumerable<DiscountItemSummary> Discounts { get; set; } = new List<DiscountItemSummary>();

    public IEnumerable<CouponItemSummary> Counpons { get; set; } = new List<CouponItemSummary>();
}