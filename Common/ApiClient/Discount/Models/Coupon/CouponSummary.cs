namespace ApiClient.Discount.Models.Coupon;

public class CouponSummary
{
    public int Id { get; set; }

    public string? CatalogName { get; set; }

    public CatalogType Type { get; set; }

    public string? Description { get; set; }

    public int Amount { get; set; }
}