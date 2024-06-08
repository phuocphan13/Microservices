using ApiClient.Basket.Models;
using Basket.API.Entitites;

namespace Basket.API.Extensions.ModelExtensions;

public static class ShoppingCartExtensions
{
    public static CartDetail ToDetail(this ShoppingCart entity)
    {
        return new CartDetail
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            Items = entity.Items.Select(x => new CartItem
            {
                Price = x.Price,
                ProductCode = x.ProductCode,
                Quantity = x.Quantity
            }).ToList(),
            TotalPrice = entity.TotalPrice
        };
    }

    public static ShoppingCart ToEntityFromSave(this SaveCartRequestBody requestBody)
    {
        return new()
        {
            UserId = requestBody.UserId,
            Items = requestBody.Items.Select(x => new ShoppingCartItem()
            {
                Price = x.Price,
                ProductCode = x.ProductCode,
                Quantity = x.Quantity,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestBody.UserId
            }).ToList()
        };
    }
}