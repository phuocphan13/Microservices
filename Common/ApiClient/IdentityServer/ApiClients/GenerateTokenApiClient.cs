using ApiClient.Common;
using ApiClient.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.IdentityServer;

public interface IGenerateTokenApiClient
{
    Task<ApiDataResult<TokenResponse>> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken);
}

public class GenerateTokenApiClient : CommonApiClient, IGenerateTokenApiClient
{
    public GenerateTokenApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiDataResult<TokenResponse>> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken)
    {
        var url = $"{GetIdentityServerBaseUrl()}/api{ApiUrlConstants.GenerateToken}";

        var result = await PostAsync<TokenResponse>(url, request, cancellationToken);

        return result;
    }
}