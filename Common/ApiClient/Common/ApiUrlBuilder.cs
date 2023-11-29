namespace ApiClient.Common;

public static class ApiUrlBuilder
{
    public static string AddQueryStringParameter(this string url, string key, string value, bool isFirst = false)
    {
        if (isFirst)
        {
            url += "&";
        }

        url += $"{key}={value}";

        return url;
    }
}