using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Services.Caches;
using Catalog.API.Services.Caches.Filters;
using Catalog.API.Services.Grpc;
using Catalog.API.Services.Workers;

namespace Catalog.API.Extensions.AppBuilder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ICatalogService, CatalogService>();
        
        services.AddScoped<IProductHistoryService, ProductHistoryService>();

        // Cached Services
        services.AddSingleton<IProductCachedService, ProductCachedService>();
        services.AddSingleton<ISubCategoryCachedService, SubCategoryCachedService>();

        services.AddSingleton<IProductCachedFilter, ProductCachedFilter>();
        
        // Grpc
        services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
        
        // Worker Services
        services.AddHostedService<ProductCachedWorkerService>();
        services.AddHostedService<RefreshCacheWorkerService>();

        return services;
    }
}