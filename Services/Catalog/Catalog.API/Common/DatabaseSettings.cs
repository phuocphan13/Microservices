namespace Catalog.API.Common;

public interface IDatabaseSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
    public bool IsRebuildSchema { get; set; }
}

public class DatabaseSettings : IDatabaseSettings
{
    public string? DatabaseName { get; set; }
    public string? ConnectionString { get; set; }
    public bool IsRebuildSchema { get; set; }
}