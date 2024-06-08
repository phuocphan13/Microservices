namespace ApiClient.Basket.Models;

public class CartItem
{
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string ProductCode { get; set; } = null!;
}