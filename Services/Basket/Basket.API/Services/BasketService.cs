using ApiClient.Basket.Models;
using ApiClient.DirectApiClients.Catalog;
using Basket.API.Entitites;
using Basket.API.Extensions.ModelExtensions;
using Basket.API.Repositories;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<BasketDetail?> GetBasketAsync(string userId, CancellationToken cancellationToken = default);
    Task<BasketDetail> SaveBasketAsync(SaveBasketRequestBody basket, CancellationToken cancellationToken = default);
    Task DeleteBasketAsync(string userId, CancellationToken cancellationToken = default);
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

    public async Task<BasketDetail?> GetBasketAsync(string userName, CancellationToken cancellationToken)
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

    public async Task<BasketDetail> SaveBasketAsync(SaveBasketRequestBody basket, CancellationToken cancellationToken)
    {
        var entity = await _basketRepository.GetBasket(basket.UserId, cancellationToken);

        if (entity is null)
        {
            entity = basket.ToEntityFromSave();
        }
        else
        {
            entity.ToEntityFromUpdate(basket);
        }

        await _basketRepository.SaveBasket(entity, cancellationToken);

        return entity.ToDetail();
    }

    public async Task DeleteBasketAsync(string userId, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(userId, cancellationToken);
    }

    #region Internal Functions
    private async Task UpdateBasketItemsInternalAsync(Entitites.Basket basket, CancellationToken cancellationToken)
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