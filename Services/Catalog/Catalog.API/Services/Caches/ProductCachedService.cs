using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Models;
using Catalog.API.Repositories;
using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public interface IProductCachedService
{
    Task<List<ProductCachedModel>?> QueryCachedProductsAsync(Func<ProductCachedModel, bool> predicate, CancellationToken cancellationToken = default);
    Task<List<ProductCachedModel>?> GetCachedProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductCachedModel?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ProductCachedModel?> UpdateHasChangeProductAsync(string id, CancellationToken cancellationToken = default);
    Task RefreshCachedProductsAsync(CancellationToken cancellationToken = default);
}

public class ProductCachedService : IProductCachedService
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    // Update this when Create/Update/Delete
    private static List<string> _productIdsFilter = new();

    private readonly IRedisDbFactory _redisCache;
    private readonly IRepository<Product> _productRepository;

    public ProductCachedService(IRedisDbFactory redisCache, IRepository<Product> productRepository)
    {
        _redisCache = redisCache;
        _productRepository = productRepository;

        GetProductIdsAsync(default).GetAwaiter().GetResult();
    }
    
    public async Task RefreshCachedProductsAsync(CancellationToken cancellationToken)
    {
        foreach (var id in _productIdsFilter)
        {
            await GetCachedProductByIdAsync(id, cancellationToken);
        }
    }
    
    public async Task<List<ProductCachedModel>?> QueryCachedProductsAsync(Func<ProductCachedModel, bool> predicate, CancellationToken cancellationToken)
    {
        List<ProductCachedModel>? products = await GetCachedProductsAsync(cancellationToken);

        if (products is null)
        {
            return null;
        }

        return products.Where(predicate).ToList();
    }

    public async Task<List<ProductCachedModel>?> GetCachedProductsAsync(CancellationToken cancellationToken)
    {
        List<ProductCachedModel> products = new();

        foreach (var id in _productIdsFilter)
        {
            var product = await GetCachedProductByIdAsync(id, cancellationToken);

            if (product is not null)
            {
                products.Add(product);
            }
        }

        return products;
    }

    public async Task<ProductCachedModel?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _redisCache.GetAsync<ProductCachedModel>(id, cancellationToken);

        if (product is null || product.HasChange)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                product = await _redisCache.GetAsync<ProductCachedModel>(id, cancellationToken);

                if (product is not null)
                {
                    return product;
                }

                var entity = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (entity is null)
                {
                    return null;
                }

                var cachedProduct = entity.ToCachedModel();
                cachedProduct.HasChange = false;
                cachedProduct.LastUpdated = DateTime.UtcNow;

                await _redisCache.SetAsync(id, cachedProduct, null, cancellationToken);

                product = cachedProduct;
            }
            finally
            {
                semaphore.Release();
            }
        }

        return product;
    }

    public async Task<ProductCachedModel?> UpdateHasChangeProductAsync(string id, CancellationToken cancellationToken)
    {
        var product = await GetCachedProductByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.HasChange = true;

        await _redisCache.SetAsync(id, product, null, cancellationToken);

        return product;
    }

    private async Task<List<string>> GetProductIdsAsync(CancellationToken cancellationToken)
    {
        if (_productIdsFilter.Count == 0)
        {
            _productIdsFilter = await _productRepository.GetEntityIdsAsync(cancellationToken);
        }

        return _productIdsFilter;
    }
}