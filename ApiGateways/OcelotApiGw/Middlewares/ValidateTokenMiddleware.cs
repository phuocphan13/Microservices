using ApiClient.DirectApiClients.Identity;
using Core.Common.Helpers;

namespace OcelotApiGw.Middlewares;

public class ValidateTokenMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var bearerToken = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            context.Response.StatusCode = 401;
        }
        
        var token = bearerToken.Split(" ")[1];

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = 401;
        }

        var payloadJObject = JsonHelpers.DeserializeFromBase64(token.Split(".")[1]);

        var userName = payloadJObject.GetValue("sub")!.ToString();
        
        if (string.IsNullOrWhiteSpace(userName))
        {
            context.Response.StatusCode = 401;
        }
        
        var service = context.RequestServices.GetService<IIdentityInternalClient>();
        
        if (service is null)
        {
            context.Response.StatusCode = 401;
            
            return;
        }
        
        var result = await service.ValidateTokenAsync(userName, token, context.RequestAborted);
        
        if (result is null || !result.Result.IsValid)
        {
            context.Response.StatusCode = 401;
        }

        await next(context);
    }
}