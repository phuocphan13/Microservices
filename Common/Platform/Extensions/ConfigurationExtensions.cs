using Microsoft.Extensions.Configuration;

namespace Platform.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConfigurationValue(
        this IConfiguration configuration,
        string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return configuration[key] ?? string.Empty;
    }
}