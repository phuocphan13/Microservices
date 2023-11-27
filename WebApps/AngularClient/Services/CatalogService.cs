using ApiClient.Catalog.ApiClient;
using ApiClient.Catalog.Models;

namespace AngularClient.Services;

public interface ICatalogService
{
    Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken = default);
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
}