using EventBus.Messages.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Messages;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddEventBusServices(this IServiceCollection services)
    {
        services.AddScoped<IQueueService, QueueService>();

        // services.AddMassTransit(x =>
        // {
        //     x.AddEntityFrameworkOutbox<RegistrationDbContext>(o =>
        //     {
        //         o.UsePostgres();
        //     });
        // });
        
        return services;
    }
}