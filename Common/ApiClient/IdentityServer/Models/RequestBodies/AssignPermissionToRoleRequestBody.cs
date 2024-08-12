namespace ApiClient.IdentityServer.Models.RequestBodies;

public class AssignPermissionToRoleRequestBody
{
    public string RoleId { get; set; } = null!;

    public string ApplicationId { get; set; } = null!;
    
    public string FeatureId { get; set; } = null!;
}