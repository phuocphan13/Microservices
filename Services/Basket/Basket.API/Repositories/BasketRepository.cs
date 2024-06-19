using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories;

public interface IBasketRepository
{
    Task<Entitites.Basket?> GetBasket(string userId, CancellationToken cancellationToken = default);
    Task<Entitites.Basket?> SaveBasket(Entitites.Basket basket, CancellationToken cancellationToken = default);
    Task DeleteBasket(string userName, CancellationToken cancellationToken = default);
}

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
    }

    public async Task<Entitites.Basket?> GetBasket(string userId, CancellationToken cancellationToken)
    {
        var basket = await _redisCache.GetStringAsync(userId, cancellationToken);
        
        if(string.IsNullOrWhiteSpace(basket))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<Entitites.Basket>(basket);
    }

    public async Task<Entitites.Basket?> SaveBasket(Entitites.Basket basket, CancellationToken cancellationToken)
    {
        await _redisCache.SetStringAsync(basket.UserId, JsonConvert.SerializeObject(basket), cancellationToken);

        return await GetBasket(basket.UserId, cancellationToken);
    }

    public async Task DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        await _redisCache.RemoveAsync(userName, cancellationToken);
    }
}
