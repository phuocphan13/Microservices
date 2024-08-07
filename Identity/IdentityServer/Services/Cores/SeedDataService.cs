using IdentityServer.Common;
using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services.Cores;

public interface ISeedDataService
{
    Task SeedDataForInitializeAsync(CancellationToken cancellationToken = default);
}

public class SeedDataService : ISeedDataService
{
    private readonly AuthenContext _dbcontext;
    private readonly UserManager<Account> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public SeedDataService(AuthenContext dbcontext, UserManager<Account> userManager, RoleManager<Role> roleManager)
    {
        _dbcontext = dbcontext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedDataForInitializeAsync(CancellationToken cancellationToken)
    {
        var accounts = GetAccounts();
        await SeedForAccountsAsync(accounts, cancellationToken);

        var roles = GetRoles();
        await SeedDataForRolesAsync(roles);

        var globalAdminRole = roles.First(x => x.Name == "GlobalAdmin");
        await SeedDataForUserRoleAsync(globalAdminRole, cancellationToken);

        var applications = GetApplications(globalAdminRole);
        await SeedDataForApplicationsAsync(applications, cancellationToken);

        await _dbcontext.SaveChangesAsync(cancellationToken);
    }

    private List<Account> GetAccounts()
    {
        return new List<Account>()
        {
            new()
            {
                UserName = "UserA",
                FullName = "User A",
                Email = "UserA@ms.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                UserName = "UserB",
                FullName = "User B",
                Email = "UserB@ms.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                UserName = "Admin",
                FullName = "Admin",
                Email = "Admin@ms.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
        };
    }

    private List<Role> GetRoles()
    {
        return new List<Role>()
        {
            new()
            {
                Name = "GlobalAdmin",
                Description = "Global Admin Role",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
            },
            new()
            {
                Name = "Admin",
                Description = "Admin Role",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
            }
        };
    }

    private List<Application> GetApplications(Role role)
    {
        return new List<Application>()
        {
            new()
            {
                ExternalId = Guid.NewGuid(),
                Name = PermissionConstants.Application.Name.CatalogApi,
                Description = "Catalog Api",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
                Features = GetListFeature("CatalogApi"),
                Role = role
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                Name = PermissionConstants.Application.Name.DiscountApi,
                Description = "Discount Api",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
                Role = role
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                Name = PermissionConstants.Application.Name.DiscountGrpc,
                Description = "Discount Grpc",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
                Role = role
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                Name = PermissionConstants.Application.Name.BasketApi,
                Description = "Basket Api",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
                Role = role
            },
            new()
            {
                ExternalId = Guid.NewGuid(),
                Name = PermissionConstants.Application.Name.OrderingApi,
                Description = "Order Api",
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = "Lucifer",
                Role = role
            },
        };
    }

    private List<Feature> GetListFeature(string applicationName)
    {
        return applicationName switch
        {
            PermissionConstants.Application.Name.CatalogApi => new List<Feature>()
            {
                new()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = PermissionConstants.Feature.CatalogApi.GetAllProducts,
                    Description = "Get All Products",
                },
                new()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = PermissionConstants.Feature.CatalogApi.GetProductById,
                    Description = "Get Product By Id",
                },
                new()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = PermissionConstants.Feature.CatalogApi.CreateProduct,
                    Description = "Create Product",
                },
                new()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = PermissionConstants.Feature.CatalogApi.UpdateProduct,
                    Description = "Update Product",
                },
                new()
                {
                    ExternalId = Guid.NewGuid(),
                    Name = PermissionConstants.Feature.CatalogApi.DeleteProduct,
                    Description = "Delete Product",
                },
            },
            _ => new()
        };
    }

    private async Task SeedForAccountsAsync(List<Account> accounts, CancellationToken cancellationToken)
    {
        var entities = await _dbcontext.Account.ToListAsync(cancellationToken);

        if (entities.Any())
        {
            return;
        }

        foreach (var item in accounts)
        {
            await _userManager.CreateAsync(item, "Ab123456_");
        }
    }

    private async Task SeedDataForRolesAsync(List<Role> roles)
    {
        foreach (var item in roles)
        {
            await _roleManager.CreateAsync(item);
        }
    }

    private async Task SeedDataForUserRoleAsync(Role role, CancellationToken cancellationToken)
    {
        var accounts = await _dbcontext.Account.ToListAsync(cancellationToken);
        
        foreach (var acc in accounts)
        {
            await _userManager.AddToRoleAsync(acc, role.Name);
        }
    }

    private async Task SeedDataForApplicationsAsync(List<Application> applications, CancellationToken cancellationToken)
    {
        foreach (var item in applications)
        {
            await _dbcontext.Application.AddAsync(item, cancellationToken);
        }
    }
}