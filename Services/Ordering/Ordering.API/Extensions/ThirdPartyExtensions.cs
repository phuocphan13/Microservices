using EventBus.Messages;
using EventBus.Messages.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.EventBusConsumer;

namespace Ordering.API.Extensions;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParties(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddHostedService<MassTransitConsoleHostedService>();

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
            x.AddConsumer<BasketCheckoutConsumer>();
        });
        
        return services;
    }
}