namespace ApiClient.DirectApiClients.Identity.RequestBodies;

public class AssignRoleToUserRequestBody
{
    public string UserId { get; set; } = null!;
    
    public string RoleId { get; set; } = null!;
}