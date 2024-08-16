using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Services.Caches;
using Catalog.API.Services.Grpc;

namespace Catalog.API.Extensions.AppBuilder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ICatalogService, CatalogService>();

        // Cached Services
        services.AddScoped<IProductCachedService, ProductCachedService>();
        
        // Grpc
        services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();

        return services;
    }
}