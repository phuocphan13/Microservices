using ApiClient.Basket.Models;
using ApiClient.Catalog.Product;
using Basket.API.Entitites;
using Basket.API.Extensions.ModelExtensions;
using Basket.API.Repositories;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<CartDetail?> GetBasketAsync(string userName, CancellationToken cancellationToken = default);
    Task<ShoppingCart?> UpdateBasketAsync(UpdateCartRequestBody basket, CancellationToken cancellationToken = default);
    Task DeleteBasketAsync(string userName, CancellationToken cancellationToken = default);
}

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductApiClient _productApiClient;

    public BasketService(IBasketRepository basketRepository, IProductApiClient productApiClient)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _productApiClient = productApiClient ?? throw new ArgumentNullException(nameof(productApiClient));
    }

    public async Task<CartDetail?> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(userName, cancellationToken);

        if (basket is null)
        {
            return null;
        }

        await UpdateBasketItemsInternalAsync(basket, cancellationToken);

        return basket.ToDetail();
    }

    public async Task<ShoppingCart?> UpdateBasketAsync(UpdateCartRequestBody basket, CancellationToken cancellationToken)
    {
        var entity = await _basketRepository.GetBasket(basket.UserName!, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        entity.ToEntityFromUpdate(basket);

        await _basketRepository.UpdateBasket(entity, cancellationToken);

        return await _basketRepository.GetBasket(basket.UserName!, cancellationToken);
    }

    public async Task DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(userName, cancellationToken);
    }

    #region Internal Functions
    private async Task UpdateBasketItemsInternalAsync(ShoppingCart basket, CancellationToken cancellationToken)
    {
        if (basket.SessionDate < DateTime.Now.AddHours(-24))
        {
            var productCodes = basket.Items.Select(x => x.ProductCode);

            var result = await _productApiClient.GetProductsByListCodesAsync(productCodes, cancellationToken);

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
    }
    #endregion
}