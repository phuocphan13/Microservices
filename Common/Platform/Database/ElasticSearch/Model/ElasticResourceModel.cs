using System.Text.Json.Serialization;

namespace Platform.Database.ElasticSearch.Model;

public class ElasticResourceModel
{
    [JsonPropertyName("service")]
    public ElasticServiceModel Service { get; set; } = null!;
}

public class ElasticServiceModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("version")]
    public string Version { get; set; } = null!;


    [JsonPropertyName("instance")]
    public ElasticServiceInstance Instance { get; set; } = null!;
}

public class ElasticTelemetryModel
{
    [JsonPropertyName("sdk")]
    public ElasticTelemetrySdk Sdk { get; set; } = null!;
}

public class ElasticTelemetrySdk
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("version")]
    public string Version { get; set; } = null!;
}

public class ElasticServiceInstance
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}