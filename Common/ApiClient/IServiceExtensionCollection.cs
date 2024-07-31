using ApiClient.Basket;
using ApiClient.Catalog.Catalog;
using ApiClient.Catalog.Category;
using ApiClient.Catalog.Product;
using ApiClient.Catalog.SubCategory;
using ApiClient.DirectApiClients.Catalog;
using ApiClient.Discount.Coupon;
using ApiClient.IdentityServer;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient;

public static class IServiceExtensionCollection
{
    public static IServiceCollection AddCatalogApiClient(this IServiceCollection services)
    {
        services.AddScoped<IProductApiClient, ProductApiClient>();
        services.AddScoped<ICategoryApiClient, CategoryApiClient>();
        services.AddScoped<ISubCategoryApiClient, SubCategoryApiClient>();
        services.AddScoped<ICatalogApiClient, CatalogApiClient>();

        return services;
    }
    
    public static IServiceCollection AddDiscountApiClient(this IServiceCollection services)
    {
        services.AddScoped<ICouponApiClient, CouponApiClient>();

        return services;
    }

    public static IServiceCollection AddBasketApiClient(this IServiceCollection services)
    {
        services.AddScoped<IBasketApiClient, BasketApiClient>();
        
        return services;
    }

    public static IServiceCollection AddCatalogInternalClient(this IServiceCollection services)
    {
        services.AddScoped<IProductInternalClient, ProductInternalClient>();

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