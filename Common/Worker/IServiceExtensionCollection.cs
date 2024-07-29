using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.Entities;
using Worker.Services;

namespace Worker;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WorkerContext>(options =>
            options.UseSqlServer(configuration["Worker:WorkerConnectionString"]));

        services.AddSingleton<IRunStateService, RunStateService>();
        
        return services;
    }
}