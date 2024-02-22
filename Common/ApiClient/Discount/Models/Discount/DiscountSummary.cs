namespace ApiClient.Discount.Models.Discount;

public class DiscountSummary
{
    public int Id { get; set; }
    
    public string? Description { get; set; }

    public int Amount { get; set; }

    public DiscountEnum? Type { get; set; }

    public string? CatalogCode { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}