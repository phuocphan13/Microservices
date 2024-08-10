using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public interface IRoleService
{
    Task<List<Role>> GetRoleByUserIdAsync(string userId);
}

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<Account> _userManager;

    public RoleService(RoleManager<Role> roleManager, UserManager<Account> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<Role>> GetRoleByUserIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var roleNames = await _userManager.GetRolesAsync(user);

        var roles = new List<Role>();

        foreach (var name in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(name);

            roles.Add(role);
        }

        return roles;
    }
}