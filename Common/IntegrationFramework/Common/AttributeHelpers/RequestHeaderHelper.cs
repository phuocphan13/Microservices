using Microsoft.AspNetCore.Http;

namespace IntegrationFramework.Common.AttributeHelpers;

public static class RequestHeaderHelper
{
    public static string GetAuthorizationToken(IHeaderDictionary header)
    {
        var authorization = header["Authorization"];
        var token = authorization.ToString().Split(" ")[1];

        return token;
    }

    public static string GetPayloadToken(IHeaderDictionary header)
    {
        var token = GetAuthorizationToken(header);

        return token.Split(".")[1];
    }
}