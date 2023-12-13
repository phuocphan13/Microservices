using ApiClient.Catalog.ApiClient.Catalog.Product;
using ApiClient.Catalog.Models.Catalog.Product;
using ApiClient.Common;

namespace AngularClient.Services.Catalog.Product;

public interface ICatalogService
{
    Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductById(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class ProductService : ICatalogService
{
    private readonly ICatalogApiClient _catalogApiClient;

    public ProductService(ICatalogApiClient catalogApiClient)
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
    }

    public async Task<ProductDetail?> GetProductById(string id, CancellationToken cancellationToken)
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

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.CreateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.UpdateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _catalogApiClient.DeleteProductAsync(id, cancellationToken);

        return result;
    }
}