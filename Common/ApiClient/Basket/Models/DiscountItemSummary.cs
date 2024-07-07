namespace ApiClient.Basket.Models;

public class DiscountItemSummary
{
    public DiscountEnum Type { get; set; }

    public string CatalogCode { get; set; } = null!;
    
    public decimal Amount { get; set; }
}