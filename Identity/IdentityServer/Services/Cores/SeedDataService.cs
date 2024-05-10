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

    public SeedDataService(AuthenContext dbcontext, UserManager<Account> userManager)
    {
        _dbcontext = dbcontext;
        _userManager = userManager;
    }

    public async Task SeedDataForInitializeAsync(CancellationToken cancellationToken)
    {
        var accounts = GetAccounts();
        await SeedForAccountsAsync(accounts, cancellationToken);

        await _dbcontext.SaveChangesAsync(cancellationToken);
    }

    private List<Account> GetAccounts()
    {
        return new List<Account>()
        {
            new()
            {
                UserName = "An.Nguyen",
                FullName = "Nguyễn Trần An",
                Email = "An.Nguyen@gdw.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                UserName = "Binh.Le",
                FullName = "Lê Thanh Bình",
                Email = "Binh.Le@gdw.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                UserName = "Admin",
                FullName = "Admin",
                Email = "Admin@gdw.com",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
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
}