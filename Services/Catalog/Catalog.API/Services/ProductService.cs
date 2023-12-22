using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Common.Consts;
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

    public async Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var apiDataResult = new ApiDataResult<ProductDetail>();
        var isExisted = await _productRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);

        if (isExisted)
        {
            apiDataResult.Message = ResponseMessages.Product.ProductExisted(requestBody.Name);
        }

        var product = requestBody.ToCreateProduct();

        await MappingProductInternalAsync(product, requestBody, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.CategoryId))
        {
            apiDataResult.Message = ResponseMessages.Product.PropertyNotExisted("Category", requestBody.Category);
            return apiDataResult;
        }

        if (string.IsNullOrWhiteSpace(product.SubCategoryId))
        {
            apiDataResult.Message = ResponseMessages.Product.PropertyNotExisted("SubCategory", requestBody.SubCategory);
            return apiDataResult;
        }

        await _productRepository.CreateEntityAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            apiDataResult.Message = ResponseMessages.Product.CreatFailure;
            return apiDataResult;
        }

        apiDataResult.Data = product.ToDetail();
        return apiDataResult;
    }

    public async Task<ApiDataResult<ProductDetail>> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var apiDataResult = new ApiDataResult<ProductDetail>();
        var product = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        if (product is null)
        {
            apiDataResult.Message = ResponseMessages.Product.NotFound;
            return apiDataResult;
        }

        product.ToUpdateProduct(requestBody);
        await MappingProductInternalAsync(product, requestBody, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.CategoryId))
        {
            apiDataResult.Message = ResponseMessages.Product.PropertyNotExisted("Category", requestBody.Category);
            return apiDataResult;
        }

        if (string.IsNullOrWhiteSpace(product.SubCategoryId))
        {
            apiDataResult.Message = ResponseMessages.Product.PropertyNotExisted("SubCategory", requestBody.SubCategory);
            return apiDataResult;
        }

        var result = await _productRepository.UpdateEntityAsync(product, cancellationToken);

        if (!result)
        {
            apiDataResult.Message = ResponseMessages.Product.UpdateFailed;
            return apiDataResult;
        }

        apiDataResult.Data = product.ToDetail();
        return apiDataResult;
    }

    // --> ApiStatusResult
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

        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.CategoryId == category.Id && string.Equals(x.Name, requestBody.SubCategory), cancellationToken);

        if (subCategory is null)
        {
            return;
        }

        entity.CategoryId = category.Id;
        entity.SubCategoryId = subCategory.Id;
    }
    #endregion
}