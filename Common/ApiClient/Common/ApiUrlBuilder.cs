namespace ApiClient.Common;

public static class ApiUrlBuilder
{
    public static string AddQueryStringParameter(this string url, string key, string value)
    {
        var isSecond = url.Contains('?');

        if (isSecond)
        {
            url += "&";
        }
        else
        {
            url += "?";
        }
        

        url += $"{key}={value}";

        return url;
    }

    public static string AddDataInUrl(this string url, string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        var paramPlace = "{" + key.ToLowerInvariant() + "}";
        url = url.Replace(paramPlace, value);
        
        return url;
    }
}