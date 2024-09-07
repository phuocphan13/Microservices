using Microsoft.AspNetCore.Http;

namespace Core.Common.Helpers;

public static class RequestHeaderHelper
{
    public static string GetAuthorizationToken(IHeaderDictionary header)
    {
        var authorization = header.Authorization;

        if (string.IsNullOrWhiteSpace(authorization))
        {
            return string.Empty;
        }

        var authorizationArray = authorization.ToString().Split(" ");
        
        if (authorizationArray.Length < 2)
        {
            return string.Empty;
        }
        
        var token = authorization.ToString().Split(" ")[1];

        return token;
    }

    public static string GetPayloadToken(IHeaderDictionary header)
    {
        var token = GetAuthorizationToken(header);
        
        if (string.IsNullOrWhiteSpace(token))
        {
            return string.Empty;
        }

        var tokenArray = token.Split(".");
        
        if (tokenArray.Length < 2)
        {
            return string.Empty;
        }

        return tokenArray[1];
    }
}