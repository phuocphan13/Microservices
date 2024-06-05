﻿using Basket.API.Entitites;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default);
    Task<ShoppingCart?> SaveCart(ShoppingCart cart, CancellationToken cancellationToken = default);
    Task DeleteBasket(string userName, CancellationToken cancellationToken = default);
}

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
    }

    public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken)
    {
        var basket = await _redisCache.GetStringAsync(userName, cancellationToken);
        if(string.IsNullOrWhiteSpace(basket))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart?> SaveCart(ShoppingCart cart, CancellationToken cancellationToken)
    {
        await _redisCache.SetStringAsync(cart.UserId, JsonConvert.SerializeObject(cart), cancellationToken);

        return await GetBasket(cart.UserId, cancellationToken);
    }

    public async Task DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        await _redisCache.RemoveAsync(userName, cancellationToken);
    }
}
