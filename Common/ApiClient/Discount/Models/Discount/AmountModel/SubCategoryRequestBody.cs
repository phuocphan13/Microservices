namespace ApiClient.Discount.Models.Discount.AmountModel;

public class SubCategoryRequestBody : CatalogItem
{
    public List<ProductRequestBody> Products { get; set; } = new();
}