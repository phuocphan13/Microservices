namespace ApiClient.IdentityServer.Models.RequestBodies;

public class CreateRoleRequestBody
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}