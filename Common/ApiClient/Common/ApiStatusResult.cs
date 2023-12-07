namespace ApiClient.Common;

public class ApiStatusResult
{
    public string? Message { get; set; }

    public int? InternalErrorCode
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return 200;
            }

            if (Message.Contains("not existed"))
            {
                return 404;
            }

            return 500;
        }
    }

    public int? HttpErrorCode { get; set; } = 200;
    public bool IsSuccessCode => string.IsNullOrWhiteSpace(Message) && HttpErrorCode != -1;
}