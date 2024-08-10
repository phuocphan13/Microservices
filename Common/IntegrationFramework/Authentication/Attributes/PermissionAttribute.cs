using ApiClient.DirectApiClients.Identity;
using Core.Common.Helpers;
using IntegrationFramework.Common.AttributeHelpers;
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
        var payloadJObject = JsonHelpers.DeserializeFromBase64(payload);

        if (payloadJObject.GetValue("userId") is null)
        {
            context.Result = new NotFoundResult();

            return;
        }

        var userId = payloadJObject.GetValue("userId")!.ToString();

        if (permissionService is null)
        {
            context.Result = new ForbidResult();

            return;
        }

        var hasPermission = permissionService.HasPermissionAsync(userId, _featureName, context.HttpContext.RequestAborted).GetAwaiter().GetResult();

        if (hasPermission is null || !hasPermission.Result.HasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}