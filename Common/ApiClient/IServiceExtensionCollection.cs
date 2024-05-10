using ApiClient.Catalog.Category;
using ApiClient.Catalog.Product;
using ApiClient.Catalog.SubCategory;
using ApiClient.Catalog.Validation;
using ApiClient.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
        services.AddScoped<ISubCategoryApiClient, SubCategoryApiClient>();
        services.AddScoped<IValidationApiClient, ValidationApiClient>();

        return services;
    }
    
    public static IServiceCollection AddIdentityServerServices(this IServiceCollection services)
    {
        services.AddScoped<IGenerateTokenApiClient, GenerateTokenApiClient>();
        services.AddScoped<IIdentityApiClient, IdentityApiClient>();
        return services;
    }

    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        return services;
    }
}