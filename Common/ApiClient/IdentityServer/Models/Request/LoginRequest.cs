namespace ApiClient.IdentityServer.Models.Request;

public class LoginRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool IsRememberMe { get; set; }
}