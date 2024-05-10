namespace IdentityServer.Models.Token;

public class TokenBase
{
    public string Token { get; set; } = null!;
    public DateTime ExpiredAt { get; set; }
}