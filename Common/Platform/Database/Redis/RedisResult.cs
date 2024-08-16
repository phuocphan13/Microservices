namespace Platform.Database.Redis;

public class RedisResult<T>
{
    public T? Result { get; }
    public bool WasFound { get; }

    public RedisResult(T? result)
    {
        this.Result = result;
        this.WasFound = this.Result is not null;
    }
}