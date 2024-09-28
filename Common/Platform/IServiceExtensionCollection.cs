using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Platform.Common;
using Platform.Common.Session;
using Platform.Configurations.Builders;
using Platform.Database.Redis;
using Platform.Extensions;

namespace Platform;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddPlatformCommonServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddScoped<ISessionState, SessionState>();
        services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));

        return services;
    }
    
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddSingleton<IRedisDb, RedisDb>();
        services.AddSingleton<IRedisDbFactory, RedisDbFactory>();

        services.AddHealthChecks()
            .AddRedis(configuration.GetConfigurationValue("CacheSettings:ConnectionString"), "Redis", HealthStatus.Unhealthy);

        return services;
    }
    
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbOptions(configuration);
        services.AddGrpcSettingsOptions(configuration);
        services.AddCacheSettingsOptions(configuration);
        services.AddWorkerOptions(configuration);
        services.AddJwtSettingsOptions(configuration);
        services.AddEventBusSettingsOptions(configuration);
        services.AddLoggingDbOptions(configuration);

        return services;
    }
}