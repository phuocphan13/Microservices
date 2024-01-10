using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services.Grpc;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
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
    private readonly IDiscountGrpcService _discountGrpcService;

    public ProductService(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IRepository<SubCategory> subCategoryRepository, 
        IDiscountGrpcService discountGrpcService)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }

    public async Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var a = await _discountGrpcService.GetDiscount("abc");
        
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

    public async Task<ProductDetail?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }
        
        var product = await MappingProductDetailInternalAsync(entity, cancellationToken);
        return product;
    }

    public async Task<ApiDataResult<ProductDetail>> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var apiDataResult = new ApiDataResult<ProductDetail>();
        var isExisted = await _productRepository.AnyAsync(x => x.Name == requestBody.Name, cancellationToken);

        if (isExisted)
        {
            apiDataResult.Message = ResponseMessages.Product.ProductExisted(requestBody.Name);
            return apiDataResult;
        }

        var product = requestBody.ToCreateProduct();

        var errorMessage = await MappingProductInternalAsync(product, requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            apiDataResult.Message = errorMessage;
            return apiDataResult;
        }

        product = await _productRepository.CreateEntityAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            apiDataResult.Message = ResponseMessages.Product.CreatFailure;
            return apiDataResult;
        }

        apiDataResult.Data = await MappingProductDetailInternalAsync(product, cancellationToken);
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
        var errorMessage = await MappingProductInternalAsync(product, requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            apiDataResult.Message = errorMessage;
            return apiDataResult;
        }

        var result = await _productRepository.UpdateEntityAsync(product, cancellationToken);

        if (!result)
        {
            apiDataResult.Message = ResponseMessages.Product.UpdateFailed;
            return apiDataResult;
        }

        apiDataResult.Data = await MappingProductDetailInternalAsync(product, cancellationToken);
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

    private async Task<ProductDetail> MappingProductDetailInternalAsync(Product entity, CancellationToken cancellationToken)
    {
        var product = entity.ToDetail();

        var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == entity.CategoryId, cancellationToken);
        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == entity.SubCategoryId, cancellationToken);

        product.Category = category is null ? string.Empty : category.Name;
        product.SubCategory = subCategory is null ? string.Empty : subCategory.Name;

        return product;
    }

    private async Task<string> MappingProductInternalAsync<TRequestBody>(Product entity, TRequestBody requestBody, CancellationToken cancellationToken)
        where TRequestBody : BaseProductRequestBody
    {
        var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => string.Equals(x.Name, requestBody.Category), cancellationToken);

        if (category is null)
        {
            return ResponseMessages.Product.PropertyNotExisted("Category", requestBody.Category);
        }

        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.CategoryId == category.Id && string.Equals(x.Name, requestBody.SubCategory), cancellationToken);

        if (subCategory is null)
        {
            return ResponseMessages.Product.PropertyNotExisted("SubCategory", requestBody.SubCategory);
        }

        entity.CategoryId = category.Id;
        entity.SubCategoryId = subCategory.Id;

        return string.Empty;
    }

    #endregion
}