using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetryFramework.Tracings;
using Platform.Configurations;
using Platform.Configurations.Options;

namespace OpenTelemetryFramework;

public static class IServiceExtensionCollection
{
    public static void AddOpenTelemetryLogs(this WebApplicationBuilder hostBuilder)
    {
        var openTelemetryOptions = hostBuilder.Configuration.GetSection(OptionConstants.OpenTelemetry).Get<OpenTelemetryOptions>();

        ArgumentNullException.ThrowIfNull(openTelemetryOptions);

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(openTelemetryOptions.ServiceName, serviceVersion: openTelemetryOptions.ServiceVersion);

        hostBuilder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;

            logging
                .SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(openTelemetryOptions.Endpoint);
                    // opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                });
        });
    }
    
    public static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryOptions>(configuration.GetSection(OptionConstants.OpenTelemetry));
        var openTelemetryOptions = configuration.GetSection(OptionConstants.OpenTelemetry).Get<OpenTelemetryOptions>();

        ArgumentNullException.ThrowIfNull(openTelemetryOptions);

        ActivitySourceProvider.Source = new System.Diagnostics.ActivitySource(openTelemetryOptions.ActivitySourceName);
        
        services.AddOpenTelemetry().WithTracing(tracing =>
        {
            tracing.AddSource(openTelemetryOptions.ActivitySourceName)
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .ConfigureResource(resource =>
                {
                    resource.AddService(openTelemetryOptions.ServiceName,
                        serviceVersion: openTelemetryOptions.ServiceVersion);
                });

            tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(openTelemetryOptions.Endpoint);
                });
        });

        return services;
    }

    public static IServiceCollection AddOpenTelemetryMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryOptions>(configuration.GetSection(OptionConstants.OpenTelemetry));
        var openTelemetryOptions = configuration.GetSection(OptionConstants.OpenTelemetry).Get<OpenTelemetryOptions>();

        ArgumentNullException.ThrowIfNull(openTelemetryOptions);

        services.AddOpenTelemetry().WithMetrics(metric =>
        {
            metric.AddMeter(openTelemetryOptions.ServiceName);
            metric.AddMeter("Microsft.AspNetCore.Hosting");
            metric.AddMeter("Microsft.AspNetCore.Server.Kestrel");
            metric.AddMeter("System.Net.Http");
            metric
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
            
            metric
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(openTelemetryOptions.Endpoint);
                })
                .ConfigureResource(resource =>
                {
                    resource.AddService(serviceName: openTelemetryOptions.ServiceName,
                        serviceVersion: openTelemetryOptions.ServiceVersion);
                });
        });

        return services;
    }
}