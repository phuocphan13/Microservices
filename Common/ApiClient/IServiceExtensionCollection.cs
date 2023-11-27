using ApiClient.Catalog.ApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        return services.AddScoped<ICatalogApiClient, CatalogApiClient>();
    }
}