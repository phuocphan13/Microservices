using Catalog.API.Entities;
using Catalog.API.Repositories;

namespace Catalog.API.Services.Caches.Filters;

public interface IProductCachedFilter
{
    public List<string> ProductIds { get; set; }
}

public class ProductCachedFilter : IProductCachedFilter
{
    private readonly IRepository<Product> _productRepository;
    
    public List<string> ProductIds { get; set; }

    public ProductCachedFilter(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;

        ProductIds = GetProductIdsAsync().GetAwaiter().GetResult();
    }
    
    public async Task<List<string>> GetProductIdsAsync()
    {
        var products = await _productRepository.GetEntitiesAsync();
        return products.Select(x => x.Id).ToList();
    }
}