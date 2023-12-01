using ApiClient.Catalog.ApiClient;
using ApiClient.Catalog.Models;

namespace AngularClient.Services;

public interface ICatalogService
{
    Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail> GetProductById(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
}

public class CatalogService : ICatalogService
{
    private readonly ICatalogApiClient _catalogApiClient;

    public CatalogService(ICatalogApiClient catalogApiClient)
    {
        _catalogApiClient = catalogApiClient;
    }

    public async Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.GetProducts(cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
        //Ocelot ---> CatalogAPI
    }

    public async Task<ProductDetail> GetProductById(string id, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.GetProductByIdAsync(id, cancellationToken);
        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.GetProductByCategoryAsync(category, cancellationToken);
        
        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }
}