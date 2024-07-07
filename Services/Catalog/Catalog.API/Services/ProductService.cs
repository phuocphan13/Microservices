using ApiClient.Catalog.Product.Models;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services.Grpc;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>?> GetProductsByListCodesAsync(List<string> codes, CancellationToken cancellationToken = default);
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
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

    public async Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        bool result = propertyName switch
        {
            PropertyName.Id => await _productRepository.AnyAsync(x => x.Id == search, cancellationToken),
            PropertyName.Name => await _productRepository.AnyAsync(x => x.Name == search, cancellationToken),
            PropertyName.Code => await _productRepository.AnyAsync(x => x.ProductCode == search, cancellationToken),
            _ => false
        };

        return result;
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

    public async Task<List<ProductSummary>?> GetProductsByListCodesAsync(List<string> codes, CancellationToken cancellationToken)
    {
        var entities = await _productRepository.GetEntitiesQueryAsync(x => codes.Contains(x.ProductCode!), cancellationToken);

        if (entities is null || !entities.Any())
        {
            return null;
        }

        var products = await GetProductSummariesInternalAsync(entities, cancellationToken);

        return products;
    }

    public async Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var product = requestBody.ToCreateProduct();

        product = await _productRepository.CreateEntityAsync(product, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.Id))
        {
            return null;
        }

        var result = await MappingProductDetailInternalAsync(product, cancellationToken);
        return result;
    }

    public async Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        entity.ToUpdateProduct(requestBody);

        var product = await _productRepository.UpdateEntityAsync(entity, cancellationToken);

        if (!product)
        {
            return null;
        }

        var result = await MappingProductDetailInternalAsync(entity, cancellationToken);

        return result;
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
        var productCode = entities.Select(x => x.ProductCode);

        var categories = await _categoryRepository.GetEntitiesQueryAsync(x => categoryIds.Contains(x.Id), cancellationToken);
        var subCategories = await _subCategoryRepository.GetEntitiesQueryAsync(x => subCategoryIds.Contains(x.Id), cancellationToken);
        var discounts = await _discountGrpcService.GetAmountsAfterDiscountAsync(categories, subCategories, entities);

        

        var summaries = new List<ProductSummary>();

        foreach (var entity in entities)
        {
            var cate = categories.FirstOrDefault(x => x.Id == entity.CategoryId)?.Name;
            var subCate = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId)?.Name;
            var discount = discounts?.FirstOrDefault(x => x.CatalogCode == entity.ProductCode);

            if (discount is not null)
            {
                entity.Price -= discount.Amount;
            }

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
    #endregion
}