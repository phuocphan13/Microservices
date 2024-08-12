namespace ApiClient.IdentityServer.Models.Response;

public class AssignPermissionToRoleDetail
{
    public bool IsSuccess { get; set; }

    public string ErrorMessage { get; set; } = null!;
}