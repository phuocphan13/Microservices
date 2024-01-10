namespace ApiClient.Discount.Models.Discount;

public class BaseDiscountRequestBody
{
    public string? Description { get; set; }

    public int? Amount { get; set; }

    public DiscountEnum? Type { get; set; }

    public string? CatalogCode { get; set; }
    
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}