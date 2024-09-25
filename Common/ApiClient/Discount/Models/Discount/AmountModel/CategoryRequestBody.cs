namespace ApiClient.Discount.Models.Discount.AmountModel;

public class CategoryRequestBody : CatalogItem
{
    public ICollection<SubCategoryRequestBody> SubCategories { get; set; }  = [];
}