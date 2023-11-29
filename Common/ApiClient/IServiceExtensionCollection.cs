using ApiClient.Catalog.ApiClient;
using ApiClient.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static void AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogApiClient, CatalogApiClient>();
    }

    public static void AddCommonServices(this IServiceCollection services)
    {
    }
}