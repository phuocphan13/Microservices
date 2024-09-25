namespace ApiClient.Basket.Models;

public class BaseBasketRequestBody
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public ICollection<BasketItemSummary> Items { get; set; } = [];

    public ICollection<DiscountItemSummary> Discounts { get; set; } = [];

    public ICollection<CouponItemSummary> Counpons { get; set; } = [];
}