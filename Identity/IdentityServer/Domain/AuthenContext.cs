using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Domain;

public class AuthenContext : IdentityDbContext<Account, Role, Guid>
{
    public AuthenContext()
    {
    }

    public AuthenContext(DbContextOptions<AuthenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Account { get; set; } = null!;

    public virtual DbSet<Role> Role { get; set; } = null!;
    
    public virtual DbSet<TokenHistory> TokenHistory { get; set; } = null!;
}