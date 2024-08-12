using ApiClient.DirectApiClients.Identity.Models;
using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using IdentityServer.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services;

public interface IPermissionService
{
    Task<PermissionValidationModel> HasPermissionAsync(string userId, string feature, CancellationToken cancellationToken = default);
    Task<List<Feature>> GetPermissionByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

public class PermissionService : IPermissionService
{
    private readonly AuthenContext _authenDbContext;
    private readonly IRoleService _roleService;

    public PermissionService(AuthenContext authenDbContext, IRoleService roleService)
    {
        _authenDbContext = authenDbContext;
        _roleService = roleService;
    }
    
    public async Task<PermissionValidationModel> HasPermissionAsync(string userId, string feature, CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetRoleByUserIdAsync(userId);

        var roleIds = roles.Select(x => x.Id);

        var application = await _authenDbContext.Application
            .AsNoTracking()
            .Include(x => x.Features.Where(x => x.Name == feature))
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null || !application.Features.Any())
        {
            return new()
            {
                HasPermission = false
            };
        }

        var rolePermissions = await _authenDbContext.RolePermission
            .Where(x => roleIds.Contains(x.RoleId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return new()
        {
            HasPermission = rolePermissions.Any(x => x.ApplicationId == application.ExternalId 
                                                     && x.FeatureId == application.Features.First().ExternalId 
                                                     && roleIds.Contains(x.RoleId) 
                                                     && x.State == PermissionState.Allowed)
        };
    }

    public async Task<List<Feature>> GetPermissionByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetRoleByUserIdAsync(userId);

        var roleIds = roles.Select(x => x.Id);

        var featureIds = await _authenDbContext.RolePermission
            .Where(x => roleIds.Contains(x.RoleId))
            .AsNoTracking()
            .Select(x => x.FeatureId)
            .ToListAsync(cancellationToken);

        var features = await _authenDbContext.Feature
            .Where(x => featureIds.Contains(x.ExternalId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return features;
    }
}