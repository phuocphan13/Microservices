using ApiClient.Basket.Models;
using ApiClient.Catalog.Product;
using ApiClient.DirectApiClients.Catalog;
using Basket.API.Entitites;
using Basket.API.Extensions.ModelExtensions;
using Basket.API.Repositories;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<CartDetail?> GetBasketAsync(string userName, CancellationToken cancellationToken = default);
    Task<CartDetail> SaveCartAsync(SaveCartRequestBody cart, CancellationToken cancellationToken = default);
    Task DeleteBasketAsync(string userName, CancellationToken cancellationToken = default);
}

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductInternalClient _productInternalClient;

    public BasketService(IBasketRepository basketRepository, IProductInternalClient productInternalClient)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _productInternalClient = productInternalClient ?? throw new ArgumentNullException(nameof(productInternalClient));
    }

    public async Task<CartDetail?> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(userName, cancellationToken);

        if (basket is null)
        {
            return null;
        }

        if (basket.SessionDate < DateTime.Now.AddHours(-24))
        {
            await UpdateBasketItemsInternalAsync(basket, cancellationToken);
        }

        return basket.ToDetail();
    }

    public async Task<CartDetail> SaveCartAsync(SaveCartRequestBody cart, CancellationToken cancellationToken)
    {
        var entity = await _basketRepository.GetBasket(cart.UserId, cancellationToken);

        if (entity is null)
        {
            entity = cart.ToEntityFromSave();
        }
        else
        {
            foreach (var item in cart.Items)
            {
                if (item.Quantity == 0)
                {
                    entity.Items.RemoveAll(x => x.ProductCode == item.ProductCode);
                }
                else
                {
                    var cartItem = entity.Items.FirstOrDefault(x => x.ProductCode == item.ProductCode);

                    if (cartItem is not null)
                    {
                        cartItem.Quantity = item.Quantity;
                    }
                    else
                    {
                        entity.Items.Add(new ShoppingCartItem
                        {
                            Price = item.Price,
                            ProductCode = item.ProductCode,
                            Quantity = item.Quantity
                        });
                    }
                }
            }
        }

        await _basketRepository.SaveCart(entity, cancellationToken);

        return entity.ToDetail();
    }

    public async Task DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(userName, cancellationToken);
    }

    #region Internal Functions
    private async Task UpdateBasketItemsInternalAsync(ShoppingCart basket, CancellationToken cancellationToken)
    {
        var productCodes = basket.Items.Select(x => x.ProductCode);

        var result = await _productInternalClient.GetProductsByListCodesAsync(productCodes, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null && result.Result.Any())
        {
            foreach (var item in basket.Items)
            {
                var product = result.Result.FirstOrDefault(x => x.Code == item.ProductCode);

                if (product is not null)
                {
                    if (product.Price is not null)
                    {
                        item.Price = product.Price.Value;
                    }
                }
            }
        }
    }
    #endregion
}