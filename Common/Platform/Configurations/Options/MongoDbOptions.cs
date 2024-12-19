namespace Platform.Configurations.Options;

public sealed class MongoDbOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public bool IsRebuildSchema { get; set; }
}