namespace ApiClient.Discount.Models.Discount.AmountModel;

public class SubCategoryRequestBody : CatalogItem
{
    public ICollection<ProductRequestBody> Products { get; set; } = [];
}