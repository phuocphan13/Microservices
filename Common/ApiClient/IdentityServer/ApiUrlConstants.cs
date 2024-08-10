namespace ApiClient.IdentityServer;

public static class ApiUrlConstants
{
    //Identity Server
    public const string GenerateToken = "/Identity/GenerateToken";

    public const string Login = "/Identity/Login";
    
    public const string GenerateAccessTokenByRefreshToken = "/Identity/GenerateAccessTokenByRefreshToken";
    
    public const string ValidateToken = "/Identity/ValidationToken";
    
    //Identity
    public const string HasPermission = "/Permission/HasPermission";
}