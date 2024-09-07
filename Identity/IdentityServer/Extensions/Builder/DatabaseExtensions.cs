using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Platform.Extensions;

namespace IdentityServer.Extensions.Builder;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthenContext>(options => options.UseSqlServer(configuration.GetConfigurationValue("Configuration:ConnectionString")));
        services.AddIdentity<Account, Role>()
            .AddEntityFrameworkStores<AuthenContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}