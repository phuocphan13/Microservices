using ApiClient.Common;
using ApiClient.DirectApiClients.Identity.Models;
using ApiClient.DirectApiClients.Identity.RequestBodies;
using ApiClient.IdentityServer;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.DirectApiClients.Identity;

public interface IIdentityInternalClient
{
    Task<ApiDataResult<TokenValidationModel>> ValidateTokenAsync(string userName, string token, CancellationToken cancellationToken = default);
}

public class IdentityInternalClient : CommonApiClient, IIdentityInternalClient
{
    private readonly string _baseUrl;

    public IdentityInternalClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
        : base(httpClientFactory, configuration, sessionState)
    {
        _baseUrl = GetServiceUrl(ServiceConstants.Api.Identity);
    }
    
    public async Task<ApiDataResult<TokenValidationModel>> ValidateTokenAsync(string userName, string token, CancellationToken cancellationToken)
    {
        var url = $"{_baseUrl}{ApiUrlConstants.ValidateToken}";
        
        ValidateTokenRequestBody requestBody = new()
        {
            UserName = userName,
            Token = token
        };

        var result = await PostAsync<TokenValidationModel>(url, requestBody, cancellationToken);

        return result;
    }
}