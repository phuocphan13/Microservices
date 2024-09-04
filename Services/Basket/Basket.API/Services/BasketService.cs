using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Basket.Models;
using ApiClient.DirectApiClients.Catalog;
using AutoMapper;
using Basket.API.Extensions.ModelExtensions;
using Basket.API.Repositories;
using EventBus.Messages.Services;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<BasketSummary?> GetBasketAsync(string userId, CancellationToken cancellationToken = default);
    Task<BasketDetail?> GetBasketPreCheckoutAsync(string userId, CancellationToken cancellationToken = default);
    Task<BasketDetail> SaveBasketAsync(SaveBasketRequestBody basket, CancellationToken cancellationToken = default);
    Task DeleteBasketAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> CheckoutBasketAsync(string userId, CancellationToken cancellationToken = default);
}

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductInternalClient _productInternalClient;
    private readonly IMapper _mapper;
    private readonly IQueueService _queueService;

    public BasketService(IBasketRepository basketRepository, IProductInternalClient productInternalClient, IMapper mapper, IQueueService queueService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _productInternalClient = productInternalClient ?? throw new ArgumentNullException(nameof(productInternalClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
    }

    public async Task<bool> CheckoutBasketAsync(string userId, CancellationToken cancellationToken)
    {
        var entity = await _basketRepository.GetBasket(userId, cancellationToken);

        if (entity is null)
        {
            return false;
        }

        var isError = entity.Items.Any(x => !string.IsNullOrWhiteSpace(x.ErrorMessage));

        if (isError)
        {
            return false;
        }

        var eventMessage = _mapper.Map<BasketCheckoutMessage>(entity.ToDetail());

        try
        {
            await _queueService.SendFanoutMessageAsync(eventMessage, cancellationToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<BasketSummary?> GetBasketAsync(string userId, CancellationToken cancellationToken)
    {
        var entity = await _basketRepository.GetBasket(userId, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        await UpdateBasketItemsInternalAsync(entity, cancellationToken);

        return entity.ToSummary();
    }

    public async Task<BasketDetail?> GetBasketPreCheckoutAsync(string userId, CancellationToken cancellationToken)
    {
        var basket = await GetBasketEntityAsync(userId, cancellationToken);

        if (basket is null)
        {
            return null;
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

        await UpdateBasketItemsInternalAsync(entity, cancellationToken);
        
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
                    item.UpdateBasketItem(product);
                }
            }
        }
    }

    private async Task<Entitites.Basket?> GetBasketEntityAsync(string userId, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(userId, cancellationToken);

        if (basket is not null && basket.SessionDate < DateTime.Now.AddHours(-24))
        {
            await UpdateBasketItemsInternalAsync(basket, cancellationToken);
        }

        return basket;
    }
    #endregion
}