using ApiClient.Catalog.Product.Models;
using Basket.API.Entitites;

namespace Basket.API.Extensions.ModelExtensions;

public static class BasketItemExtensions
{
    public static void UpdateBasketItem(this BasketItem basketItem, ProductSummary product)
    {
        basketItem.Price = product.Price ?? 0;
        basketItem.CategoryCode = product.Category ?? string.Empty;
        basketItem.SubCategoryCode = product.SubCategory ?? string.Empty;

        if (basketItem.Quantity > product.Balance)
        {
            basketItem.ErrorMessage = "Not enough stock";
        }
        else
        {
            basketItem.ErrorMessage = string.Empty;
        }
    }
}