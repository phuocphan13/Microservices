using System.Reflection;
using EventBus.Messages;
using EventBus.Messages.Extensions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ReportingGateway.Api.Extensions;

public static class ThirdPartyExtensions
{
    public static IServiceCollection AddThirdParties(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //MassTransit configuration
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddHostedService<MassTransitConsoleHostedService>();

        services.AddMessageOutboxCosumer(configuration, busAction: x =>
        {
        });

        return services;
    }
}