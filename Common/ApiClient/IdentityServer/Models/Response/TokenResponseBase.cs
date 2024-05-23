namespace ApiClient.IdentityServer.Models.Response;

public class TokenResponseBase
{
    public string Token { get; set; } = null!;
    public int ExpiredAt { get; set; } 
}