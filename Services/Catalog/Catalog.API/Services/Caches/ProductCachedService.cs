using ApiClient.Common.Models.Paging;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Models;
using Catalog.API.Repositories;
using Catalog.API.Services.Caches.Filters;
using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public interface IProductCachedService
{
    Task<List<ProductCachedModel>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken = default);
    Task<List<ProductCachedModel>?> QueryCachedProductsAsync(Func<ProductCachedModel, bool> predicate, CancellationToken cancellationToken = default);
    Task<List<ProductCachedModel>?> GetCachedProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductCachedModel?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ProductCachedModel?> UpdateHasChangeProductAsync(string id, CancellationToken cancellationToken = default);
    Task RefreshCachedProductsAsync(CancellationToken cancellationToken = default);
}

public class ProductCachedService : CommonCacheService, IProductCachedService
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private const string _productKey = "Products";
    private readonly IRepository<Product> _productRepository;
    private readonly IProductCachedFilter _productCachedFilter;

    public ProductCachedService(IRedisDbFactory redisCache, IRepository<Product> productRepository, IProductCachedFilter productCachedFilter)
       : base(_productKey, redisCache)
    {
        _productRepository = productRepository;
        _productCachedFilter = productCachedFilter;
    }
    
    public async Task RefreshCachedProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetEntitiesAsync(cancellationToken);
        
        var productCacheds = products.Select(x => x.ToCachedModel()).ToList();
        
        await SetAllItemsCacheAsync(productCacheds, cancellationToken);
    }
    
    public async Task<List<ProductCachedModel>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
    {
        var products = await GetCachedProductsAsync(cancellationToken);

        if (products is null)
        {
            return new List<ProductCachedModel>();
        }

        var pagingCollection = new List<ProductCachedModel>(products.Skip(pagingInfo.Start ?? 0).Take(pagingInfo.Length ?? 10));

        return pagingCollection;
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
        List<ProductCachedModel>? products = await GetAllItemAsync<ProductCachedModel>(cancellationToken);

        if (products is not null && products.Any() && _productCachedFilter.ProductIds.Count == products.Count)
        {
            return products;
        }

        await semaphore.WaitAsync(cancellationToken);
        
        try
        {
            products = await GetAllItemAsync<ProductCachedModel>(cancellationToken);

            if (products is not null && products.Any() && _productCachedFilter.ProductIds.Count == products.Count)
            {
                return products;
            }

            products = new();
            
            var productEntities = await _productRepository.GetEntitiesAsync(cancellationToken);
            
            var productCacheds = productEntities.Select(x => x.ToCachedModel()).ToList();

            await SetAllItemsCacheAsync(productCacheds, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
        
        
        return products;
    }

    public async Task<ProductCachedModel?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await GetItemCachedByIdAsync<ProductCachedModel>(id, cancellationToken);

        if (product is null || product.HasChange)
        {
            // 499 requests blocked
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                product = await GetItemCachedByIdAsync<ProductCachedModel>(id, cancellationToken);

                if (product is not null)
                {
                    return product;
                }

                var entity = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (entity is null)
                {
                    return null;
                }

                product = await SetItemCacheAsync(entity.ToCachedModel(), cancellationToken); 
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
        var product = await UpdateHasChangeItemAsync<ProductCachedModel>(id, cancellationToken);
        
        return product;
    }
}