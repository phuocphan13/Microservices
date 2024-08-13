using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain.Entities;

namespace IdentityServer.Extensions.TransformExtensions;

public static class FeatureExtensions
{
    public static FeatureDetail ToDetail(this Feature feature, string applicationName)
    {
        return new()
        {
            ExternalId = feature.ExternalId.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            ApplicationName = applicationName
        };
    }
}