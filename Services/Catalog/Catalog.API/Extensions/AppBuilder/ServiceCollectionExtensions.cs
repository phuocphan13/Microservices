using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Services.Caches;
using Platform.Database.MongoDb;

namespace Catalog.API.Extensions.AppBuilder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {
        services.AddSingleton<SemaphoreSlim>(_ => new SemaphoreSlim(1, 1));
        
        services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<ICatalogService, CatalogService>();
        
        services.AddScoped<IProductHistoryService, ProductHistoryService>();

        // Cached Services
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}