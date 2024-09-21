using Logging.Domain.Migrations;
using Microsoft.Extensions.Options;
using Platform.Configurations.Options;

namespace Logging.Middlewares;

public class MigrationMiddleware
{
    private readonly RequestDelegate _next;

    public MigrationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext, IOptions<LoggingDbOptions> options)
    {
        if (options.Value.IsRebuildSchema)
        {
            // Create and run the migration service
            var migrationService = new MigrationService(options.Value.ConnectionString);
            migrationService.MigrateUp(); // Apply all pending migrations

            Console.WriteLine("Migrations applied successfully.");
        }

        return _next(httpContext);
    }
}

public static class MigrationMiddlewareExtensions
{
    public static IApplicationBuilder UseMigrationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MigrationMiddleware>();
    }
} 