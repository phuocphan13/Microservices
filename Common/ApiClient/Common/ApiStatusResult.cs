namespace ApiClient.Common;

public class ApiStatusResult
{
    public string? Message { get; set; }
    public int? InternalErrorCode { get; set; }
    public bool IsSuccessCode => string.IsNullOrWhiteSpace(Message);
}