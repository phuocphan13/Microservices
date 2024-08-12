namespace ApiClient.IdentityServer.Models.RequestBodies;

public class RemoveRoleToUserRequestBody
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;
}