using System.Net;

namespace Platform.ApiBuilder;

public static class HttpStatusCodeExtensions
{
    public static bool IsSuccess(this HttpStatusCode httpStatusCode)
    {
        int num = (int)httpStatusCode;
        return num >= 200 && num < 300;
    }
}