namespace Platform.ApiBuilder;

public class ApiStatusResult : ApiResult
{
    public string? Message { get; set; }
    public int? InternalErrorCode { get; set; }
}

public sealed class ApiStatusResult<T> : ApiStatusResult
{
    public T? Additional { get; set; }

    public string GetAdditionalString()
    {
        string? additional = null;
        if (this.Additional is not null)
        {
            additional = this.Additional.ToString();
        }

        return additional ?? string.Empty;
    }
}