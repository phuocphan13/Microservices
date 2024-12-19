namespace Platform.Configurations.Options;

public sealed class CacheSettingsOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    
    public int DefaultDb { get; set; }
}