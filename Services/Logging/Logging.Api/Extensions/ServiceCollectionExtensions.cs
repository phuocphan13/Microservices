using Logging.Services;

namespace Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<ILogService, LogService>();
        
        return services;
    }
}