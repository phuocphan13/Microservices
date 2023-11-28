using ApiClient.Catalog.Models;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var entities = await _productRepository.GetProductsAsync(cancellationToken);

        return entities.Select(x => x.ToSummary()).ToList();
    }

    public async Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var entities = await _productRepository.GetProductsQueryAsync(x => x.Category == category, cancellationToken);

        return entities.Select(x => x.ToSummary()).ToList();
    }

    public async Task<ProductDetail> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetProductFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity.ToDetail();
    }

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = requestBody.ToCreateProduct();

        await _productRepository.CreateProductAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            return null;
        }

        return product.ToDetail();
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.ToUpdateProduct(requestBody);

        var result = await _productRepository.UpdateProductAsync(product, cancellationToken);

        if (!result)
        {
            return null;
        }

        return product.ToDetail();
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        var result = await _productRepository.DeleteProductAsync(id, cancellationToken);

        return result;
    }
}