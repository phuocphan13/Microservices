using Catalog.API.Models.Cached;
using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public class CommonCacheService
{
    protected readonly IRedisDbFactory _redisCache;
    private readonly string _key;

    protected CommonCacheService(string key, IRedisDbFactory redisCache)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));    
        }
        
        _redisCache = redisCache;
        _key = key;
    }

    protected async Task<List<T>?> GetAllItemAsync<T>(CancellationToken cancellationToken) 
        where T : class, new()
    {
        return await _redisCache.GetAsync<List<T>>(_key, cancellationToken);
    }

    protected async Task SetAllItemsCacheAsync<T>(List<T> items, CancellationToken cancellationToken)
        where T : class, new()
    {
        await _redisCache.SetAsync(_key, items, TimeSpan.FromMinutes(5), cancellationToken);
    }
    
    protected async Task<T> SetItemCacheAsync<T>(T item, CancellationToken cancellationToken)
        where T : class, new()
    {
        var items = await GetAllItemAsync<T>(cancellationToken);

        if (items is null || items.Count == 0)
        {
            items = [ item ];
        }
        else
        {
            items.Add(item);
        }

        await _redisCache.SetAsync(_key, items, null, cancellationToken);

        return item;
    }
}