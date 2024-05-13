namespace ApiClient.IdentityServer.Models.Request;

public class GenerateAccessTokenByRefreshTokenRequest
{
    public string AccountId { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}