using ApiClient.Common;
using ApiClient.IdentityServer.Models.RequestBodies;
using ApiClient.IdentityServer.Models.Response;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.IdentityServer;

public interface IIdentityApiClient
{
    Task<ApiDataResult<LoginResponse>> LoginAsync(LoginRequestBody request, CancellationToken cancellationToken = default);
    Task<ApiDataResult<AccessTokenDetail>> GenerateAccessTokenByRefreshTokenAsync(GenerateAccessTokenByRefreshTokenRequestBody request, CancellationToken cancellationToken = default);
}

public class IdentityApiClient : CommonApiClient, IIdentityApiClient
{
    public IdentityApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiDataResult<LoginResponse>> LoginAsync(LoginRequestBody request, CancellationToken cancellationToken)
    {
        var url = $"{GetIdentityServerBaseUrl()}/api{ApiUrlConstants.Login}";

        var result = await PostAsync<LoginResponse>(url, request, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<AccessTokenDetail>> GenerateAccessTokenByRefreshTokenAsync(GenerateAccessTokenByRefreshTokenRequestBody request, CancellationToken cancellationToken)
    {
        var url = $"{GetIdentityServerBaseUrl()}/api{ApiUrlConstants.GenerateAccessTokenByRefreshToken}";

        var result = await PostAsync<AccessTokenDetail>(url, request, cancellationToken);

        return result;
    }
}