namespace ApiClient.Common;

public static class ApiUrlBuilder
{
    public static string AddQueryStringParameter(this string url, string key, string value, bool isFirst = false)
    {
        if (isFirst)
        {
            url += "?";
        }
        else
        {
            url += "&";
        }
        

        url += $"{key}={value}";

        return url;
    }

    public static string AddDataInUrl(this string url, string key, string value)
    {
        var paramPlace = "{" + key.ToLower() + "}";
        url = url.Replace(paramPlace, value);
        
        return url;
    }
}