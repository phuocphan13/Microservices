using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public interface IFeatureService
{
}

public class FeatureService : IFeatureService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<Account> _userManager;
    private readonly AuthenContext _authenContext;

    public FeatureService(RoleManager<Role> roleManager, UserManager<Account> userManager, AuthenContext authenContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _authenContext = authenContext;
    }
}