using ApiClient.Catalog.Product;
using ApiClient.Catalog.Product.Models;
using Platform.ApiBuilder;

namespace AngularClient.Services;

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
    private readonly IProductApiClient _productApiClient;

    public ProductService(IProductApiClient productApiClient)
    {
        _productApiClient = productApiClient;
    }

    public async Task<List<ProductSummary>?> GetProductsAsync(CancellationToken cancellationToken)
    {
        var result = await _productApiClient.GetProducts(cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result.ToList();
        }

        return null;
    }

    public async Task<ProductDetail?> GetProductById(string id, CancellationToken cancellationToken)
    {
        var result = await _productApiClient.GetProductByIdAsync(id, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<List<ProductSummary>?> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var result = await _productApiClient.GetProductByCategoryAsync(category, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result.ToList();
        }

        return null;
    }

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _productApiClient.CreateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _productApiClient.UpdateProductAsync(requestBody, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _productApiClient.DeleteProductAsync(id, cancellationToken);

        return result;
    }
}