namespace ApiClient.Discount.Models.Discount;

public class AmountDiscountRequestBody
{
    public string? Type { get; set; }

    public string? CatalogCode { get; set; }
    public List<SubCategoryListRequest> SubCategoryList { get; set; } = new List<SubCategoryListRequest>();
}