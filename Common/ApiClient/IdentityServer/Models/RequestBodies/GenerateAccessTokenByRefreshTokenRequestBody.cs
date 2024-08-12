namespace ApiClient.IdentityServer.Models.RequestBodies;

public class GenerateAccessTokenByRefreshTokenRequestBody
{
    public string AccountId { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}