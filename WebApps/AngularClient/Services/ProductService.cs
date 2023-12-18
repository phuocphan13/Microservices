using ApiClient.Catalog.ApiClient.Catalog;
using ApiClient.Catalog.ApiClient.Catalog.Product;
using ApiClient.Catalog.Models.Catalog.Product;
using ApiClient.Common;

namespace AngularClient.Services.Catalog;

public interface IProductService
{
    Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductById(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class ProductService : IProductService
{
    private readonly IProductApiClient _ProductApiClient;

    public ProductService(IProductApiClient ProductApiClient)
    {
        _ProductApiClient = ProductApiClient;
    }

    public async Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.GetProducts(cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ProductDetail?> GetProductById(string id, CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.GetProductByIdAsync(id, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.GetProductByCategoryAsync(category, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.CreateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.UpdateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _ProductApiClient.DeleteProductAsync(id, cancellationToken);

        return result;
    }
}