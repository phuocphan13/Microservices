using ApiClient.DirectApiClients.Identity;
using Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationFramework.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _featureName;

    public PermissionAttribute(string featureName)
    {
        _featureName = featureName;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permissionService = context.HttpContext.RequestServices.GetService<IPermissionInternalClient>();
        var header = context.HttpContext.Request.Headers;

        var payload = RequestHeaderHelper.GetPayloadToken(header);
        
        if (string.IsNullOrWhiteSpace(payload))
        {
            context.Result = new ForbidResult();

            return;
        }
        
        var payloadJObject = JsonHelpers.DeserializeFromBase64(payload);

        var userId = payloadJObject.GetValue("userId");

        if (userId is null)
        {
            context.Result = new NotFoundObjectResult("UserId not found");

            return;
        }

        if (permissionService is null)
        {
            context.Result = new ForbidResult();

            return;
        }

        var hasPermission = permissionService.HasPermissionAsync(userId.ToString(), _featureName, context.HttpContext.RequestAborted).GetAwaiter().GetResult();

        if (hasPermission is null || !hasPermission.Result.HasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}