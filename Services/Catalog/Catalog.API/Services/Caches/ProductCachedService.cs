using Catalog.API.Models;
using Newtonsoft.Json;
using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public interface IProductCachedService
{
    Task<List<ProductCachedModel>?> GetCachedProducts(CancellationToken cancellationToken = default);
    Task<ProductCachedModel?> GetCachedProductById(string id, CancellationToken cancellationToken = default);
}

public class ProductCachedService : IProductCachedService
{
    private readonly IRedisDbFactory _redisCache;
    private readonly string KeyProducts = "Products";
    
    public ProductCachedService(IRedisDbFactory redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<List<ProductCachedModel>?> GetCachedProducts(CancellationToken cancellationToken)
    {
        var products = await _redisCache.GetAsync<List<ProductCachedModel>>(KeyProducts, cancellationToken);

        if (products is null)
        {
            return null;
        }

        return products;
    }
    
    public async Task<ProductCachedModel?> GetCachedProductById(string id, CancellationToken cancellationToken)
    {
        var product = await _redisCache.GetAsync<ProductCachedModel>(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        return product;
    }
}