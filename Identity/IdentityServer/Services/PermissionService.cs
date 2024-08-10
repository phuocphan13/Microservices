using ApiClient.DirectApiClients.Identity.Models;
using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
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

    public PermissionService(AuthenContext authenDbContext, IRoleService roleService, UserManager<Account> userManager)
    {
        _authenDbContext = authenDbContext;
        _roleService = roleService;
    }
    
    public async Task<PermissionValidationModel> HasPermissionAsync(string userId, string feature, CancellationToken cancellationToken)
    {
        var features = await GetPermissionByUserIdAsync(userId, cancellationToken);

        return new()
        {
            HasPermission = features.Any(x => x.Name == feature)
        };
    }

    public async Task<List<Feature>> GetPermissionByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetRoleByUserIdAsync(userId);

        var roleIds = roles.Select(x => x.Id);

        var features = await _authenDbContext.Role.Where(x => x.IsActive && roleIds.Contains(x.Id))
            .AsNoTracking()
            .Include(x => x.Applications)
            .ThenInclude(x => x.Features)
            .SelectMany(x => x.Applications)
            .SelectMany(x => x.Features)
            .ToListAsync(cancellationToken);

        return features;
    }
}