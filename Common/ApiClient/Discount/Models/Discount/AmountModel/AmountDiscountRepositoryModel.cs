namespace ApiClient.Discount.Models.Discount.AmountModel;

public class AmountDiscountRepositoryModel
{
    public string Type { get; set; } = null!;
    public ICollection<string> CatalogCodes { get; set; } = [];
}