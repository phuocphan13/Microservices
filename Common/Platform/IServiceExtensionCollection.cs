using Microsoft.Extensions.DependencyInjection;
using Platform.Common;
using Platform.Common.Session;
using Platform.Database.Redis;

namespace Platform;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddPlatformCommonServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ISessionState, SessionState>();
        services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));

        return services;
    }
    
    public static IServiceCollection AddRedisServices(this IServiceCollection services)
    {
        // services.AddSingleton<IRedisDb, RedisDb>();
        services.AddSingleton<IRedisDbFactory, RedisDbFactory>();

        return services;
    }
}