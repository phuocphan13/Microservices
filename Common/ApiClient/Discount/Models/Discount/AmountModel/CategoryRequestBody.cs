namespace ApiClient.Discount.Models.Discount.AmountModel;

public class CategoryRequestBody : CatalogItem
{
    public List<SubCategoryRequestBody> SubCategories { get; set; }  = new();
}