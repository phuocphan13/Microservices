namespace ApiClient.IdentityServer.Models.Response;

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public int AccessTokenExpires { get; set; }
    public string? TokenType { get; set; }
    public string? RefreshToken { get; set; }
    public int RefreshTokenExpires { get; set; }
}