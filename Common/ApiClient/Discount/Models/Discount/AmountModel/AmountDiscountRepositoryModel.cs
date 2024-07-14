namespace ApiClient.Discount.Models.Discount.AmountModel;

public class AmountDiscountRepositoryModel
{
    public string Type { get; set; } = null!;
    public List<string> CatalogCodes { get; set; } = new();
}