using System.Configuration;
using Logging.Domain.Repositories;
using Logging.Services;

namespace Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        
        // services.AddTransient<ILogRepository, LogRepository>(provider =>
        //     new LogService("MySQLConnection")));

        services.AddTransient<ILogRepository, LogRepository>();
        
        services.AddScoped<ILogService, LogService>();
        
        return services;
    }
}