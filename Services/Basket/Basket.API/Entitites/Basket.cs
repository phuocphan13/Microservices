namespace Basket.API.Entitites;

public class Basket
{
    public Basket()
    {

    }

    public Basket(string username)
    {
        this.UserName = username;
    }
    
    //Todo, Should concern about UserId with UserName
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;

    //Todo, Using SessionState to store the session date
    public DateTime SessionDate { get; set; }
    
    public List<BasketItem> Items { get; set; } = new();

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
