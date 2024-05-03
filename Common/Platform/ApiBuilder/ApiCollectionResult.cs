namespace Platform.ApiBuilder;

public class ApiCollectionResult<T> : ApiResult
{
    public IEnumerable<T> Result { get; set; }

    public string? Message { get; set; }

    public ApiCollectionResult(IEnumerable<T> result) => this.Result = result;
}