using ApiClient.Basket.Models;

namespace Basket.API.Extensions.ModelExtensions;

public static class ShoppingBasketExtensions
{
    public static BasketDetail ToDetail(this Entitites.Basket entity)
    {
        return new BasketDetail
        {
            UserId = entity.UserId,
            UserName = entity.UserName,
            Items = entity.Items.Select(x => new BasketItemSummary
            {
                Price = x.Price,
                ProductCode = x.ProductCode,
                Quantity = x.Quantity
            }).ToList(),
            TotalPrice = entity.TotalPrice
        };
    }

    public static Entitites.Basket ToEntityFromSave(this SaveBasketRequestBody requestBody)
    {
        return new()
        {
            UserId = requestBody.UserId,
            Items = requestBody.Items.Select(x => new Entitites.BasketItem()
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