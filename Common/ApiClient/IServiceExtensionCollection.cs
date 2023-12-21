using ApiClient.Catalog.Category;
using ApiClient.Catalog.Product;
using ApiClient.Catalog.SubCategory;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static void AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
        services.AddScoped<ISubCategoryApiClient, SubCategoryApiClient>();
    }

    public static void AddCommonServices(this IServiceCollection services)
    {
    }
}