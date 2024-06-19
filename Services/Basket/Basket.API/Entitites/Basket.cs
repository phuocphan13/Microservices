using ApiClient.Discount.Enum;
using Basket.API.Extensions.ModelExtensions;

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
    
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;

    public DateTime SessionDate { get; set; }
    
    public List<BasketItem> Items { get; set; } = new();

    public List<DiscountItem> Discounts { get; set; } = new();
    
    public List<CouponItem> Coupons { get; set; } = new();

    public decimal TotalPrice
    {
        //Todo: Should Recalculate the total price of the basket, Should Apply Discounts or Coupons first
        get
        {
            decimal totalPrice = 0;
            
            foreach (var item in Items)
            {
                item.Price = item.CalculateItemPrice(Discounts, Coupons);
                
                totalPrice += item.Price * item.Quantity;
            }
            
            return totalPrice;
        }
    }
}