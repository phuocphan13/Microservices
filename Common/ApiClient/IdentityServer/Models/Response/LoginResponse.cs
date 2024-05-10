namespace ApiClient.IdentityServer.Models.Response;

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public DateTime AccessTokenExpires { get; set; }
    public string? TokenType { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}