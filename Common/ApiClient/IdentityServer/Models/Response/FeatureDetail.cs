namespace ApiClient.IdentityServer.Models.Response;

public class FeatureDetail
{
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ApplicationName { get; set; } = null!;
}