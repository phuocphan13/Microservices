namespace ApiClient.Basket.Models;

public class CartSummary
{
    public string? UserName { get; set; }

    public List<CartItem> Items { get; set; } = new();

    public decimal TotalPrice { get; set; }
}