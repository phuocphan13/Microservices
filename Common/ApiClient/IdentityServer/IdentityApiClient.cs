using ApiClient.Common;
using ApiClient.IdentityServer.Models.Request;
using ApiClient.IdentityServer.Models.Response;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.IdentityServer;

public interface IIdentityApiClient
{
    Task<ApiDataResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}

public class IdentityApiClient : CommonApiClient, IIdentityApiClient
{
    public IdentityApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiDataResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var url = $"{GetIdentityServerBaseUrl()}/api{ApiUrlConstants.Login}";

        var result = await PostAsync<LoginResponse>(url, request, cancellationToken);

        return result;
    }
}