using ApiClient.Discount.Enum;

namespace Basket.API.Entitites;

public class CouponItem : BaseEntity
{
    public CouponEnum Type { get; set; }

    public string CatalogCode { get; set; } = null!;
    
    public decimal Amount { get; set; }
}