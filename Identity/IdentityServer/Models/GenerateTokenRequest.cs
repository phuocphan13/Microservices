namespace IdentityServer.Models;

public class GenerateTokenRequest
{
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public Dictionary<string, string> CustomClaims { get; set; } = new();
}