using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.Configurations.Options;

namespace Platform.Configurations.Builders;

public static class OptionBuilder
{
    public static void AddMongoDbOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(OptionConstants.MongoDb));
    }

    public static void AddGrpcSettingsOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GrpcSettingsOptions>(configuration.GetSection(OptionConstants.GrpcSettings));
    }

    public static void AddCacheSettingsOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheSettingsOptions>(configuration.GetSection(OptionConstants.CacheSettings));
    }

    public static void AddWorkerOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WorkerOptions>(configuration.GetSection(OptionConstants.Worker));
    }

    public static void AddJwtSettingsOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettingsOptions>(configuration.GetSection(OptionConstants.JwtSettings));
    }

    public static void AddEventBusSettingsOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusSettingsOptions>(configuration.GetSection(OptionConstants.EventBusSettings));
    }
}