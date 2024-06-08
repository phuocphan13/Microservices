using EventBus.Messages.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Messages;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services)
    {
        services.AddScoped<IQueueService, QueueService>();
        
        return services;
    }
}