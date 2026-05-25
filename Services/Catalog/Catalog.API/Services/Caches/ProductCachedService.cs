using ApiClient.Catalog.Product.Models;
using ApiClient.Common.Models.Paging;
using Platform.Database.Redis;

namespace Catalog.API.Services.Caches;

public interface IProductCachedService
{
    Task<List<ProductDetail>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken = default);
    Task<List<ProductDetail>?> QueryCachedProductsAsync(Func<ProductDetail, bool> predicate, CancellationToken cancellationToken = default);
    Task<List<ProductDetail>> GetCachedProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task RefreshCachedProductsAsync(CancellationToken cancellationToken = default);
}

public class ProductCachedService : CommonCacheService, IProductCachedService
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private const string _productKey = "Products";
    private readonly IProductService _productService;

    public ProductCachedService(IRedisDbFactory redisCache, IProductService productService)
       : base(_productKey, redisCache)
    {
        _productService = productService;
    }
    
    public async Task RefreshCachedProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductDetailsAsync(cancellationToken);
        
        await SetAllItemsCacheAsync(products, cancellationToken);
    }
    
    public async Task<List<ProductDetail>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
    {
        var products = await GetCachedProductsAsync(cancellationToken);

        if (products is null)
        {
            return [ ];
        }

        var pagingCollection = new List<ProductDetail>(products.Skip(pagingInfo.Start ?? 0).Take(pagingInfo.Length ?? 10));

        return pagingCollection;
    }
    
    public async Task<List<ProductDetail>?> QueryCachedProductsAsync(Func<ProductDetail, bool> predicate, CancellationToken cancellationToken)
    {
        List<ProductDetail>? products = await GetCachedProductsAsync(cancellationToken);

        if (products is null)
        {
            return null;
        }

        return products.Where(predicate).ToList();
    }

    public async Task<List<ProductDetail>> GetCachedProductsAsync(CancellationToken cancellationToken)
    {
        List<ProductDetail>? products = await GetAllItemAsync<ProductDetail>(cancellationToken);

        if (products is not null && products.Count != 0)
        {
            return products;
        }

        await semaphore.WaitAsync(cancellationToken);
        
        try
        {
            products = await GetAllItemAsync<ProductDetail>(cancellationToken);

            if (products is not null && products.Count != 0)
            {
                return products;
            }

            products = [ ];
            
            var productEntities = await _productService.GetProductDetailsAsync(cancellationToken);
            
            await SetAllItemsCacheAsync(productEntities, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
        
        
        return products;
    }

    public async Task<ProductDetail?> GetCachedProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await GetItemCachedByIdAsync(id, cancellationToken);

        if (product is null)
        {
            // 499 requests blocked
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                product = await GetItemCachedByIdAsync(id, cancellationToken);

                if (product is not null)
                {
                    return product;
                }

                var entity = await _productService.GetProductByIdAsync(id, cancellationToken);

                if (entity is null)
                {
                    return null;
                }

                product = await SetItemCacheAsync(entity, cancellationToken); 
            }
            finally
            {
                semaphore.Release();
            }
        }

        return product;
    }
    
    private async Task<ProductDetail?> GetItemCachedByIdAsync(string id, CancellationToken cancellationToken)
    {
        var items = await GetAllItemAsync<ProductDetail>(cancellationToken);

        if (items is null)
        {
            return null;
        }

        var item = items.FirstOrDefault(x => x.Id == id);

        return item;
    }
}