using System.Text.Json.Serialization;

namespace Platform.Database.ElasticSearch.Model;

public class ElasticScopeModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("version")]
    public string Version { get; set; } = null!;
}