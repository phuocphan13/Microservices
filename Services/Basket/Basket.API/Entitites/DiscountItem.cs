namespace Basket.API.Entitites;

public class DiscountItem : BaseEntity
{
    public DiscountEnum Type { get; set; }

    public string CatalogCode { get; set; } = null!;
    
    public decimal Amount { get; set; }
}