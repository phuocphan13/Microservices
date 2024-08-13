namespace ApiClient.IdentityServer.Models.Response;

public class RemoveRoleDetail
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = null!;
}