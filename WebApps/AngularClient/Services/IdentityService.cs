using ApiClient.IdentityServer;
using ApiClient.IdentityServer.Models;

namespace AngularClient.Services;

public interface IIdentityService
{
    Task<TokenResponse?> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken = default);
}

public class IdentityService : IIdentityService
{
    private readonly IGenerateTokenApiClient _generateTokenApiClient;

    public IdentityService(IGenerateTokenApiClient generateTokenApiClient)
    {
        _generateTokenApiClient = generateTokenApiClient;
    }

    public async Task<TokenResponse?> GenerateTokenAsync(GenerateTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _generateTokenApiClient.GenerateTokenAsync(request, cancellationToken);

        if (result.IsSuccessStatusCode || result.Result is null)
        {
            return null;
        }

        return result.Result;
    }
}