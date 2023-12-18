using ApiClient.Catalog.ApiClient.Catalog;
using ApiClient.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static void AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
    }

    public static void AddCommonServices(this IServiceCollection services)
    {
    }
}