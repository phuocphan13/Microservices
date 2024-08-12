namespace ApiClient.IdentityServer.Models.RequestBodies;

public class LoginRequestBody
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool IsRememberMe { get; set; }
    public string? Grant_type { get; set; }
    public string? Scope { get; set; }
}