namespace ApiClient.IdentityServer.Models.Response;

public class RemoveRoleToUserDetail
{
    public bool IsSuccess { get; set; }

    public string ErrorMessage { get; set; } = null!;
}