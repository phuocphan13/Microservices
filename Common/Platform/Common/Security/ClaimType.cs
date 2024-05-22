namespace Platform.Common.Security;

public static class ClaimType
{
    public static string Issuer => "iss";

    public static string Name => "name";

    public static string Emails => "emails";

    public static string ObjectIdentifier
    {
        get => "http://schemas.microsoft.com/identity/claims/objectidentifier";
    }

    public static string Scope => "http://schemas.microsoft.com/identity/claims/scope";

    public static string Audience => "aud";

    public static string TrustedFrameworkPolicy => "tfp";
}