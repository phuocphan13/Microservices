namespace ApiClient.Basket.Models;

public class BaseCartRequestBody
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public List<CartItem> Items { get; set; } = new();
}