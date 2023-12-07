namespace ApiClient.Common;

public class ApiDataResult<T>
    where T : class, new()
{
    public string? Message { get; set; } 
    public int? InternalErrorCode { get; set; }

    public bool IsSuccessCode => string.IsNullOrWhiteSpace(Message);

    public T? Data { get; set; }
}