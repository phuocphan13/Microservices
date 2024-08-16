using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Platform.Database.Redis;

public interface IRedisDbFactory
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class, new();

    Task<bool> SetAsync<T>(string key, T item, TimeSpan? expiry, CancellationToken cancellationToken = default) where T : class, new();
}

public class RedisDbFactory : IRedisDbFactory, IDisposable
{
    private readonly Dictionary<string, IConnectionMultiplexer> multiplexers = new();
    private readonly SemaphoreSlim semaphore = new(1);
    private readonly string _connectionString;
    private readonly int _defaultDb;

    public RedisDbFactory(IConfiguration configuration)
    {
        _connectionString = configuration["CacheSettings:ConnectionString"];
        _defaultDb = int.Parse(configuration["CacheSettings:DefaultDb"]);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        where T : class, new()
    {
        IRedisDb redisDb = await this.CreateAsync(
            cancellationToken);

        RedisResult<T>? response = null;
        try
        {
            response = await redisDb.GetAsync<T>(key);
        }
        catch
        {
            /* Exception is deliberately ignored */
        }

        return response?.Result;
    }

    public async Task<bool> SetAsync<T>(string key, T item, TimeSpan? expiry, CancellationToken cancellationToken)
        where T : class, new()
    {
        IRedisDb redisDb = await this.CreateAsync(
            cancellationToken);

        return await redisDb.SetAsync(key, item, expiry);
    }

    private async Task<IRedisDb> CreateAsync(CancellationToken cancellationToken)
    {
        IConnectionMultiplexer? connectionMultiplexer;
        await this.semaphore.WaitAsync(cancellationToken);
        string key = $"{_defaultDb}@{_connectionString}";
        
        try
        {
            connectionMultiplexer = CheckMultiplexer(key);

            if (connectionMultiplexer is null)
            {
                connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_connectionString)
                    .ContinueWith(
                        t => t.Result as IConnectionMultiplexer,
                        CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

                multiplexers[key] = connectionMultiplexer;
            }
        }
        finally
        {
            this.semaphore.Release();
        }

        IDatabase database = connectionMultiplexer.GetDatabase(_defaultDb);
        return new RedisDb(database);
    }
    
    private IConnectionMultiplexer? CheckMultiplexer(string key)
    {
        return this.multiplexers.GetValueOrDefault(key);
    }

    private bool _isDisposed;
    private void Dispose(bool disposing)
    {
        if (!this._isDisposed)
        {
            if (disposing)
            {
                this.semaphore.Dispose();
            }

            this._isDisposed = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}