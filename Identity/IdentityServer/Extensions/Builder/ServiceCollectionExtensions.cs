using IdentityServer.Domain.Helpers;
using IdentityServer.Services;
using IdentityServer.Services.Cores;
using Platform.Database.Helpers;

namespace IdentityServer.Extensions.Builder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add services to the container.
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFeatureService, FeatureService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ITokenHandleService, TokenHandleService>();
        services.AddScoped<ITokenHistoryService, TokenHistoryService>();

        services.AddScoped<ISeedDataService, SeedDataService>();

        return services;
    }
}