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
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;

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
        
        await MappingProductInternalAsync(product, requestBody, cancellationToken);

        await _productRepository.CreateEntityAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            // entity.ValidationFailures.Add(new ValidationFailure(nameof(product.Id), "Create Product failed."));
            return null;
        }

        return product.ToDetail();
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        // IValidationResult<Product> entity = new ValidationResult<Product>(product);
        
        if (product is null)
        {
            return null;
        }

        product.ToUpdateProduct(requestBody);
        await MappingProductInternalAsync(product, requestBody, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.CategoryId) || string.IsNullOrWhiteSpace(product.SubCategoryId))
        {
            return null;
        }

        var result = await _productRepository.UpdateEntityAsync(product, cancellationToken);

        if (!result)
        {
            return null;
        }

        return product.ToDetail();
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        var result = await _productRepository.DeleteEntityAsync(id, cancellationToken);

        return result;
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

    private async Task MappingProductInternalAsync<TRequestBody>(Product entity, TRequestBody requestBody, CancellationToken cancellationToken)
        where TRequestBody : BaseProductRequestBody
    {
        var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => string.Equals(x.Name, requestBody.Category), cancellationToken);

        if (category is null)
        {
            return;
        }

        var subCateglory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.CategoryId == category.Id && string.Equals(x.Name, requestBody.SubCategory), cancellationToken);

        if (subCateglory is null)
        {
            return;
        }

        entity.CategoryId = category.Id;
        entity.SubCategoryId = subCateglory.Id;
    }
    #endregion
}