using ApiClient.Catalog.ApiClient.Catalog.Product;
using ApiClient.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static void AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogApiClient, ProductApiClient>();
    }

    public static void AddCommonServices(this IServiceCollection services)
    {
    }
}