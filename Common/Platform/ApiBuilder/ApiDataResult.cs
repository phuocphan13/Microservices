namespace Platform.ApiBuilder;

public class ApiDataResult<T> : ApiResult
{
    public ApiDataResult(T result) => this.Result = result;
    
    public T Result { get; set; }

    public string? Message { get; set; }
}