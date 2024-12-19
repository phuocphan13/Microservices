namespace Platform.Configurations.Options;

public class LoggingDbOptions
{
    public string ConnectionString { get; set; } = null!;
    public bool IsRebuildSchema { get; set; }
}