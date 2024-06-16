namespace Basket.API.Entitites;

public class BasketItem : BaseEntity
{
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    public string ProductCode { get; set; } = null!;
}
