namespace ApiClient.Common;

public class ApiDataResult<T> : ApiStatusResult
    where T : class, new()
{
    public T? Data { get; set; }
}