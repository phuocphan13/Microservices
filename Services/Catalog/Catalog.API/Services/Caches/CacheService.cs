using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public interface ICacheService
{
    Task<List<T>> GetAllAsync<T>(string key, Func<Task<List<T>>> func, CancellationToken cancellationToken = default) where T : class;

    Task<T?> GetSingleAsync<T>(string key, Func<Task<T>> func, CancellationToken cancellationToken) where T : class, new();
}

public class CacheService : ICacheService
{
    private readonly IRedisDbFactory _redisCache;
    private readonly SemaphoreSlim _semaphore;

    public CacheService(IRedisDbFactory redisCache, SemaphoreSlim semaphore)
    {
        _redisCache = redisCache;
        _semaphore = semaphore;
    }

    public async Task<List<T>> GetAllAsync<T>(string key, Func<Task<List<T>>> func, CancellationToken cancellationToken)
        where T : class
    {
        List<T>? item = await _redisCache.GetAsync<List<T>>(key, cancellationToken);

        if (item is not null && item.Count != 0)
        {
            return [];
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            item = await _redisCache.GetAsync<List<T>>(key, cancellationToken);

            if (item is not null && item.Count != 0)
            {
                return item;
            }

            item = await func();

            await _redisCache.SetAsync(key, item,  TimeSpan.FromMinutes(15), cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }


        return item;
    }

    public async Task<T?> GetSingleAsync<T>(string key, Func<Task<T>> func, CancellationToken cancellationToken)
        where T : class, new()
    {
        T? item = await _redisCache.GetAsync<T>(key, cancellationToken);

        if (item is not null)
        {
            return null;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            item = await _redisCache.GetAsync<T>(key, cancellationToken);

            if (item is not null)
            {
                return item;
            }

            item = await func();

            await _redisCache.SetAsync(key, item, TimeSpan.FromMinutes(15), cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }


        return item;
    }
}