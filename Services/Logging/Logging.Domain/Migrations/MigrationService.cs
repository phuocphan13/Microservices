using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Domain.Migrations;

public class MigrationService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(string connectionString)
    {
        _serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5() // Using MySQL 5+ driver
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationService).Assembly).For.Migrations()) // Scan for migrations in this assembly
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    public void MigrateUp()
    {
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}