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

    protected async Task<T?> GetItemCachedByIdAsync<T>(string id, CancellationToken cancellationToken)
        where T: BaseCachedModel, new()
    {
        var items = await GetAllItemAsync<T>(cancellationToken);

        return items?.FirstOrDefault(x => x.Id == id);
    }

    protected async Task<T?> GetItemCacheByNameAsync<T>(string name, CancellationToken cancellationToken) 
        where T : BaseCachedModel, new()
    {
        var item = await GetAllItemAsync<T>(cancellationToken);

        return item?.FirstOrDefault(x => x.Name == name);
    }

    //Search gần đúng like '%abc%'
    protected async Task<List<T>?> GetItemCachedApproximateIdAsync<T>(string id, CancellationToken cancellationToken)
        where T :BaseCachedModel, new()
    {
        var items = await GetAllItemAsync<T>(cancellationToken);

        return items?.Where(x => x.Id.Contains(id)).ToList();
    }

    protected async Task SetAllItemsCacheAsync<T>(List<T> items, CancellationToken cancellationToken)
        where T : BaseCachedModel, new()
    {
        await _redisCache.SetAsync(_key, items, null, cancellationToken);
    }
    
    protected async Task<T> SetItemCacheAsync<T>(T item, CancellationToken cancellationToken)
        where T : BaseCachedModel, new()
    {
        item.HasChange = false;
        item.LastUpdated = DateTime.UtcNow;

        var items = await GetAllItemAsync<T>(cancellationToken);

        if (items is null || items.Count == 0)
        {
            items = [ item ];
        }
        else
        {
            var cachedItem = items.FirstOrDefault(x => x.Id == item.Id);

            if (cachedItem is not null)
            {
                items.Remove(cachedItem);
            }

            items.Add(item);
        }

        await _redisCache.SetAsync(_key, items, null, cancellationToken);

        return item;
    }
    
    protected async Task<T?> UpdateHasChangeItemAsync<T>(string id, CancellationToken cancellationToken)
        where T : BaseCachedModel, new()
    {
        var item = await GetItemCachedByIdAsync<T>(id, cancellationToken);

        if (item is null)
        {
            return null;
        }

        item.HasChange = true;

        await _redisCache.SetAsync(id, item, null, cancellationToken);

        return item;
    }
}