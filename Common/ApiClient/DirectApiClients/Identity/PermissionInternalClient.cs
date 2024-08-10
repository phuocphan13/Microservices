using ApiClient.Common;
using ApiClient.DirectApiClients.Identity.Models;
using ApiClient.IdentityServer;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.DirectApiClients.Identity;

public interface IPermissionInternalClient
{
    Task<ApiDataResult<PermissionValidationModel>> HasPermissionAsync(string userId, string feature, CancellationToken cancellationToken = default);
}

public class PermissionInternalClient : CommonApiClient, IPermissionInternalClient
{
    private readonly string _baseUrl;

    public PermissionInternalClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
        _baseUrl = GetServiceUrl(ServiceConstants.Api.Identity);
    }

    public async Task<ApiDataResult<PermissionValidationModel>> HasPermissionAsync(string userId, string feature, CancellationToken cancellationToken)
    {
        var url = $"{_baseUrl}{ApiUrlConstants.HasPermission}";

        url = url
            .AddQueryStringParameter(nameof(userId), userId)
            .AddQueryStringParameter(nameof(feature), feature);

        var result = await GetSingleAsync<PermissionValidationModel>(url, cancellationToken);

        return result;
    }
}