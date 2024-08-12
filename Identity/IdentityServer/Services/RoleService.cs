using ApiClient.DirectApiClients.Identity.Models;
using ApiClient.IdentityServer.Models.RequestBodies;
using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using IdentityServer.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services;

public interface IRoleService
{
    Task<List<Role>> GetRoleByUserIdAsync(string userId);
    Task<AssignRoleToUserDetail> AssignRoleToUserAsync(string userId, string roleId);
    Task<RemoveRoleToUserDetail> RemoveRoleToUserAsync(string userId, string roleId);
    Task<AssignPermissionToRoleDetail> AssignPermissionToRoleAsync(AssignPermissionToRoleRequestBody requestBody, CancellationToken cancellationToken = default);
}

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<Account> _userManager;
    private readonly AuthenContext _authenContext;

    public RoleService(RoleManager<Role> roleManager, UserManager<Account> userManager, AuthenContext authenContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _authenContext = authenContext;
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
    
    public async Task<AssignRoleToUserDetail> AssignRoleToUserAsync(string userId, string roleId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        var role = await _roleManager.FindByIdAsync(roleId);

        if (role is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Role not found"
            };
        }
        
        var result = await _userManager.AddToRoleAsync(user, role.Name);

        return new()
        {
            IsSuccess = result.Succeeded
        };
    }
    
    public async Task<RemoveRoleToUserDetail> RemoveRoleToUserAsync(string userId, string roleId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        var role = await _roleManager.FindByIdAsync(roleId);

        if (role is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Role not found"
            };
        }
        
        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

        return new()
        {
            IsSuccess = result.Succeeded
        };
    }
    
    public async Task<AssignPermissionToRoleDetail> AssignPermissionToRoleAsync(AssignPermissionToRoleRequestBody requestBody, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(requestBody.RoleId);

        if (role is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Role not found"
            };
        }

        var application = await _authenContext.Application
            .Where(x => x.ExternalId == Guid.Parse(requestBody.ApplicationId))
            .AsNoTracking()
            .Include(x => x.Features.Where(x => x.ExternalId == Guid.Parse(requestBody.FeatureId)))
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Application not found"
            };
        }

        if (!application.Features.Any())
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Feature is not belong to Application"
            };
        }
        
        var rolePermission = await _authenContext.RolePermission
            .Where(x => x.RoleId == role.Id && x.ApplicationId == application.ExternalId && x.FeatureId == application.Features.First().ExternalId)
            .AsNoTracking()
            .AnyAsync(cancellationToken);
        
        if (rolePermission)
        {
            return new()
            {
                IsSuccess = false,
                ErrorMessage = "Role already has permission"
            };
        }

        var permission = new RolePermission
        {
            RoleId = role.Id,
            ApplicationId = application.ExternalId,
            FeatureId = application.Features.First().ExternalId,
            State = PermissionState.Allowed
        };

        await _authenContext.RolePermission.AddAsync(permission, cancellationToken);
        await _authenContext.SaveChangesAsync(cancellationToken);

        return new()
        {
            IsSuccess = true
        };
    }
}