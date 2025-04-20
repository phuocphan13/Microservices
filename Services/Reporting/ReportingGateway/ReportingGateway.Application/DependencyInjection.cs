using Microsoft.Extensions.DependencyInjection;
using ReportingGateway.Application.Commands;
using ReportingGateway.Application.Queries;

namespace ReportingGateway.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            // Commands
            cfg.RegisterServicesFromAssemblyContaining<GenerateReportCommandHandler>();
            
            // Queries
            cfg.RegisterServicesFromAssemblyContaining<GetReportSummariesQueryHandler>();
        });

        return services;
    }
}