namespace ApiClient.Basket.Models;

public class BasketSummary
{
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public ICollection<BasketItemSummary> Items { get; set; } = new List<BasketItemSummary>();
}