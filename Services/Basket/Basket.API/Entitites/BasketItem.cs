namespace Basket.API.Entitites;

public class BasketItem : BaseEntity
{
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string SubCategoryCode { get; set; } = null!;

    public string ProductCode { get; set; } = null!;
    
    public string ErrorMessage { get; set; } = null!;
}
