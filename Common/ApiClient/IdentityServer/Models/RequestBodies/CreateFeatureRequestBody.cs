namespace ApiClient.IdentityServer.Models.RequestBodies;

public class CreateFeatureRequestBody
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ApplicationId { get; set; } = null!;
}