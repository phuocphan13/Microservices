using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Platform.Database.Redis;

public interface IRedisDb
{
    Task<RedisResult<T>> GetAsync<T>(string key) where T : class, new();
    Task<bool> SetAsync<T>(string key, T item, TimeSpan? expiry = null) where T : class, new();
    Task<bool> RemoveAsync(string key);
    Task<bool> RefreshAsync(string key, TimeSpan? expiry = null);
    Task<List<string>> GetAllKeys();
}

public sealed class RedisDb : IRedisDb
{
    private readonly IDatabase database;

    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public RedisDb(
        IDatabase database)
    {
        ArgumentNullException.ThrowIfNull(database);

        this.database = database;
    }

    public async Task<List<string>> GetAllKeys()
    {
        var keys = new List<string>();
        RedisResult? redisValue =  await this.database.ExecuteAsync("KEYS", "*");

        if (redisValue is not null)
        {
            for (int i = 0; i < redisValue.Length; i++)
            {
                if (redisValue[i].IsNull)
                {
                    continue;
                }

                keys.Add(redisValue[i].ToString());
            }
            
        }
        
        return keys;
    }

    public Task<RedisResult<T>> GetAsync<T>(string key)
        where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return this.GetInternalAsync<T>(key);
    }

    private async Task<RedisResult<T>> GetInternalAsync<T>(string key)
        where T : class, new()
    {
        RedisKey redisKey = new(key);
        RedisValue redisValue = await this.database.StringGetAsync(redisKey);

        RedisResult<T> redisResult;
        if (redisValue.HasValue)
        {
            string data = redisValue.ToString();
            var result = JsonSerializer.Deserialize<T>(
                data,
                RedisDb.JsonSerializerOptions);

            redisResult = new(result);
        }
        else
        {
            redisResult = new RedisResult<T>(null);
        }

        return redisResult;
    }

    public Task<bool> SetAsync<T>(
        string key,
        T item,
        TimeSpan? expiry)
        where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        ArgumentNullException.ThrowIfNull(item);

        return this.SetInternalAsync(key, item, expiry);
    }

    private Task<bool> SetInternalAsync<T>(
        string key,
        T item,
        TimeSpan? expiry)
        where T : class, new()
    {
        string data = JsonSerializer.Serialize(
            item,
            RedisDb.JsonSerializerOptions);

        RedisKey redisKey = new(key);
        RedisValue redisValue = new(data);

        return this.database.StringSetAsync(redisKey, redisValue, expiry);
    }

    public Task<bool> RemoveAsync(
        string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        RedisKey redisKey = new(key);

        return this.database.KeyDeleteAsync(redisKey);
    }

    public Task<bool> RefreshAsync(
        string key,
        TimeSpan? expiry)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        RedisKey redisKey = new(key);

        return this.database.KeyExpireAsync(redisKey, expiry);
    }
}