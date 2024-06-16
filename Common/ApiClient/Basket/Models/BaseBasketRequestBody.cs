namespace ApiClient.Basket.Models;

public class BaseBasketRequestBody
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public List<BasketItemSummary> Items { get; set; } = new();
}