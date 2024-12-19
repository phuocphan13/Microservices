namespace Platform.Configurations.Options;

public class LogElasticSearchDbOptions
{
    public string Endpoint { get; set; } = null!;
    public string Index { get; set; } = null!;
}