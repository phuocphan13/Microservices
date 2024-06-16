namespace ApiClient.Basket.Models;

public class BasketSummary
{
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;

    public List<BasketItemSummary> Items { get; set; } = new();

    public decimal TotalPrice { get; set; }
}