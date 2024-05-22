using System.Security.Claims;
using System.Security.Principal;

namespace Platform.Common.Security;

public class Principal
{
    private readonly ClaimsPrincipal claimsPrincipal;

    public Principal(ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal, nameof(claimsPrincipal));
        this.claimsPrincipal = claimsPrincipal;
    }

    public string Id => this.GetClaimValue(ClaimType.ObjectIdentifier);

    public bool IsAuthenticated
    {
        get
        {
            IIdentity? identity = this.claimsPrincipal.Identity;
            return identity != null && identity.IsAuthenticated;
        }
    }

    public bool IsAuthorisedAsUser
    {
        get => !string.IsNullOrWhiteSpace(this.GetClaimValue(ClaimType.TrustedFrameworkPolicy));
    }

    public bool IsAuthorisedAsService
    {
        get => !string.IsNullOrWhiteSpace(this.GetClaimValue(ClaimType.Audience));
    }

    public string Name => this.GetClaimValue(ClaimType.Name);

    public string UserName => this.GetClaimValue(ClaimType.Emails);

    public string Issuer => this.GetClaimValue(ClaimType.Issuer);

    public bool HasClaims => this.claimsPrincipal.Claims.Any<Claim>();

    public bool HasScope(string scope) => this.HasScope(scope, this.Issuer);

    public bool HasScope(string scope, string issuer)
    {
        ArgumentNullException.ThrowIfNull(scope, nameof(scope));
        ArgumentNullException.ThrowIfNull(issuer, nameof(issuer));
        Claim? claim = this.claimsPrincipal.Claims.FirstOrDefault((Func<Claim?, bool>)(c => c?.Type == ClaimType.Scope && c.Issuer == issuer));
        bool flag = false;
        if (claim != null)
            flag = claim.Value.Split(' ').Contains(scope);
        return flag;
    }

    private string GetClaimValue(string type)
    {
        return this.claimsPrincipal.FindFirst(c => c.Type == type)?.Value ?? string.Empty;
    }
}