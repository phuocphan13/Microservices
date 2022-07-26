namespace Basket.API.Entitites;

public class ShoppingCart
{
    public ShoppingCart()
    {

    }

    public ShoppingCart(string username)
    {
        this.UserName = username;
    }

    public string? UserName { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
