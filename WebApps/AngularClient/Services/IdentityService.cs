using ApiClient.IdentityServer;
using ApiClient.IdentityServer.Models;
using ApiClient.IdentityServer.Models.RequestBodies;
using ApiClient.IdentityServer.Models.Response;

namespace AngularClient.Services;

public interface IIdentityService
{
    Task<TokenResponse?> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken = default);
    Task<LoginResponse?> LoginAsync(LoginRequestBody request, CancellationToken cancellationToken = default);
    Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(GenerateAccessTokenByRefreshTokenRequestBody request, CancellationToken cancellationToken = default);
}

public class IdentityService : IIdentityService
{
    private readonly IGenerateTokenApiClient _generateTokenApiClient;
    private readonly IIdentityApiClient _identityApiClient;

    public IdentityService(IGenerateTokenApiClient generateTokenApiClient, IIdentityApiClient identityApiClient)
    {
        _generateTokenApiClient = generateTokenApiClient;
        _identityApiClient = identityApiClient;
    }

    public async Task<TokenResponse?> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _generateTokenApiClient.GenerateTokenAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode || result.Result is null)
        {
            return null;
        }

        return result.Result;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequestBody request, CancellationToken cancellationToken)
    {
        var result = await _identityApiClient.LoginAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode || result.Result is null)
        {
            return null;
        }

        return result.Result;
    }
    
    public async Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(GenerateAccessTokenByRefreshTokenRequestBody request, CancellationToken cancellationToken)
    {
        var result = await _identityApiClient.GenerateAccessTokenByRefreshTokenAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode || result.Result is null)
        {
            return null;
        }

        return result.Result;
    }
}