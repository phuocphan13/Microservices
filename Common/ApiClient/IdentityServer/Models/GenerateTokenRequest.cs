namespace ApiClient.IdentityServer.Models;

public class GenerateTokenRequest
{
    public string? Email { get; set; }
    public string? UserId { get; set; }
}