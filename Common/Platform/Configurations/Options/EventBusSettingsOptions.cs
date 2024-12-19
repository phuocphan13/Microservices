namespace Platform.Configurations.Options;

public sealed class EventBusSettingsOptions
{
    public string HostAddress { get; set; } = string.Empty;

    public string ConnectionString { get; set; } = string.Empty;

    public string IsRebuildSchema { get; set; } = string.Empty;
}