using FluentMigrator;
using Logging.Domain.Entities;

namespace Logging.Domain.Migrations;

// 17/09/2024 Version 01 => 1.0
[Migration(1709202401)]
public class CreateLoggingTable : Migration
{
    public override void Up()
    {
        Create.Table("Logs")
            .WithColumn(nameof(Log.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(Log.Text)).AsString(1000).NotNullable()
            .WithColumn(nameof(Log.CreatedAt)).AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn(nameof(Log.CreatedBy)).AsString(100).NotNullable()
            .WithColumn(nameof(Log.Type)).AsInt16()
            .WithColumn(nameof(Log.Meter)).AsInt16()
            .WithColumn(nameof(Log.ObjectName)).AsString(100).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Logs");
    }
}