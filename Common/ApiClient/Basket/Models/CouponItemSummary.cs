using ApiClient.Discount.Enum;

namespace ApiClient.Basket.Models;

public class CouponItemSummary
{
    public CouponEnum Type { get; set; }

    public string CatalogCode { get; set; } = null!;

    public decimal Amount { get; set; }
}