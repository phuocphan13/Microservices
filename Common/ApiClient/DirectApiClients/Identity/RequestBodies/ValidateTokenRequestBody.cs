namespace ApiClient.DirectApiClients.Identity.RequestBodies;

public class ValidateTokenRequestBody
{
    public string Token { get; set; } = null!;

    public string UserName { get; set; } = null!;
}