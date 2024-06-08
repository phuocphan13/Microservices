namespace Basket.API.Entitites;

public class ShoppingCartItem : BaseEntity
{
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    public string ProductCode { get; set; } = null!;
}
