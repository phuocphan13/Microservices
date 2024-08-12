namespace ApiClient.DirectApiClients.Identity.Models;

public class AssignRoleToUserDetail
{
    public bool IsSuccess { get; set; }
    
    public string ErrorMessage { get; set; } = null!;
}