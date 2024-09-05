namespace Platform.Configurations.Options;

public sealed class WorkerOptions
{
    public string WorkerConnectionString { get; set; } = string.Empty;
    
    public bool IsRebuildSchema { get; set; }
}