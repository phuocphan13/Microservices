namespace Platform.Configurations.Options;

public sealed class JwtSettingsOptions
{
    public string Issuer { get; set; } = string.Empty;
    
    public string Audience { get; set; } = string.Empty;
    
    public string Key { get; set; } = string.Empty;
    
    public int LifeTime { get; set; }
}