namespace ApiClient.Basket.Models;

public class BasketItemSummary
{
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string ProductCode { get; set; } = null!;

    public bool IsRemove { get; set; } = false;
}