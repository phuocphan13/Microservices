using ApiClient.Catalog.Models;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
=======
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
>>>>>>> Stashed changes
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategoryService> _subCategoryRepository;

    public ProductService(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IRepository<SubCategory> subCategoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
    }

    public async Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var entities = await _productRepository.GetEntitiesAsync(cancellationToken);

        var summaries = await GetProductSummariesInternalAsync(entities, cancellationToken);
        
        return summaries;
    }

    public async Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var categoryEntity = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == category, cancellationToken);

        if (categoryEntity is not null)
        {
            var entities = await _productRepository.GetEntitiesQueryAsync(x => x.CategoryId == categoryEntity.Id, cancellationToken);

            return await GetProductSummariesInternalAsync(entities, cancellationToken);
        }

        return new List<ProductSummary>();
    }

    public async Task<ProductDetail> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity.ToDetail();
    }

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = requestBody.ToCreateProduct();

        await _productRepository.CreateEntityAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            return null;
        }

        return product.ToDetail();
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.ToUpdateProduct(requestBody);

        var result = await _productRepository.UpdateEntityAsync(product, cancellationToken);

        if (!result)
        {
            return null;
        }

        return product.ToDetail();
    }

    // --> ApiStatusResult
    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var outcome = new ApiStatusResult();

        if (product is null)
        {
            outcome.Message = ResponseMessages.Delete.NotFound;

            return outcome;
        }

        var result = await _productRepository.DeleteEntityAsync(id, cancellationToken);

        if (result == false)
        {
            outcome.Message = ResponseMessages.Delete.DeleteFailed;
        }

        return outcome;
    }

    #region Internal Functions
    private async Task<List<ProductSummary>> GetProductSummariesInternalAsync(List<Product> entities, CancellationToken cancellationToken)
    {
        var categoryIds = entities.Select(x => x.CategoryId);
        var subCategoryIds = entities.Select(x => x.SubCategoryId);

        var categories = await _categoryRepository.GetEntitiesQueryAsync(x => categoryIds.Contains(x.Id), cancellationToken);
        var subCategories = await _subCategoryRepository.GetEntitiesQueryAsync(x => subCategoryIds.Contains(x.Id), cancellationToken);

        var summaries = new List<ProductSummary>();

        foreach (var entity in entities)
        {
            var cate = categories.FirstOrDefault(x => x.Id == entity.CategoryId)?.Name;
            var subCate = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId)?.Name;

            summaries.Add(entity.ToSummary(cate, subCate));
        }

        return summaries;
    }
    #endregion
}