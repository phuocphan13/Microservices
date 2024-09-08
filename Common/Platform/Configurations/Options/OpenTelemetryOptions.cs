namespace Platform.Configurations.Options;

public sealed class OpenTelemetryOptions
{
    public string ActivitySourceName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceVersion { get; set; } = string.Empty;
    public string ProthemousEndpoint { get; set; } = string.Empty;
    public string JaegerEndpoint { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}