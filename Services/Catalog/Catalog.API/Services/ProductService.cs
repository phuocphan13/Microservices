using ApiClient.Catalog.Product.Models;
using ApiClient.Catalog.ProductHistory.Models;
using ApiClient.Common.Models.Paging;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Services.Caches;
using Catalog.API.Services.Grpc;
using Platform.Database.MongoDb;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<PagingCollection<ProductSummary>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductDetail?> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<List<ProductSummary>?> GetProductsByListCodesAsync(List<string> codes, CancellationToken cancellationToken = default);
    Task<ProductDetail?> CreateProductAsync(CreateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ProductDetail?> UpdateProductAsync(UpdateProductRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> ReduceProductBalanceAsync(List<ReduceProductBalanceRequestBody> requestBodies, CancellationToken cancellationToken = default);
    Task<List<ProductDetail>> GetProductDetailsAsync(CancellationToken cancellationToken = default);
}

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;
    private readonly IDiscountGrpcService _discountGrpcService;
    private readonly IProductHistoryService _productHistoryService;
    private readonly ICacheService _cacheService;
    
    public ProductService(
        IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<SubCategory> subCategoryRepository,
        IDiscountGrpcService discountGrpcService, IProductHistoryService productHistoryService, ICacheService cacheService)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        _productHistoryService = productHistoryService ?? throw new ArgumentNullException(nameof(productHistoryService));
        _cacheService = cacheService;
    }

    public async Task<bool> ReduceProductBalanceAsync(List<ReduceProductBalanceRequestBody> requestBodies, CancellationToken cancellationToken)
    {
        var codes = requestBodies.Select(x => x.ProductCode).ToList();
        var products = await _productRepository.GetEntitiesQueryAsync(x => codes.Contains(x.ProductCode!), cancellationToken);

        if (products is null)
        {
            return false;
        }
        
        List<AddProductBalanceRequestBody> addProductBalances = new();

        foreach (var entity in products)
        {
            var body = requestBodies.FirstOrDefault(x => x.ProductCode == entity.ProductCode);

            if (body is null)
            {
                continue;
            }

            entity.Balance -= body.Quantity;

            addProductBalances.Add(new()
            {
                Balance = body.Quantity,
                Id = entity.Id
            });
        }

        await _productRepository.UpdateEntitiesAsync(products, cancellationToken);
        await _productHistoryService.AddHistoriesAsync(addProductBalances, cancellationToken);

        return true;
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

    public async Task<PagingCollection<ProductSummary>> GetPagingProductsAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
    {
        var entities = await _productRepository.GetEntitiesPagingAsync(pagingInfo.Start, pagingInfo.Length, cancellationToken);

        var summaries = await GetProductSummariesInternalAsync(entities, cancellationToken);

        return new PagingCollection<ProductSummary>(summaries); 
    }

    public async Task<List<ProductSummary>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var entities = await _cacheService.GetAllAsync("Products", () => _productRepository.GetEntitiesAsync(cancellationToken), cancellationToken);

        var summaries = await GetProductSummariesInternalAsync(entities, cancellationToken);

        return summaries;
    }

    public async Task<List<ProductDetail>> GetProductDetailsAsync(CancellationToken cancellationToken)
    {
        var entities = await _cacheService.GetAllAsync("Products", () => _productRepository.GetEntitiesAsync(cancellationToken), cancellationToken);

        var details = await MappingProductDetailsInternalAsync(entities, cancellationToken);

        return details;
    }

    public async Task<List<ProductSummary>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var categoryEntity = await _cacheService.GetSingleAsync("Category", () => _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == category, cancellationToken), cancellationToken);

        if (categoryEntity is not null)
        {
            var entities = await _cacheService.GetAllAsync("Products", () => _productRepository.GetEntitiesQueryAsync(x => x.CategoryId == categoryEntity.Id, cancellationToken), cancellationToken);
            
            return await GetProductSummariesInternalAsync(entities, cancellationToken);
        }

        return [ ];
    }

    public async Task<ProductDetail?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _cacheService.GetSingleAsync("Product", () => _productRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken), cancellationToken);

        if (entity is null)
        {
            return null;
        }
        
        var product = await MappingProductDetailInternalAsync(entity.ToDetail(), cancellationToken);
        
        return product;
    }

    public async Task<List<ProductSummary>?> GetProductsByListCodesAsync(List<string> codes, CancellationToken cancellationToken)
    {
        
        var entities = await _cacheService.GetAllAsync("Products", () => _productRepository
            .GetEntitiesQueryAsync(x => !string.IsNullOrWhiteSpace(x.ProductCode) && codes.Contains(x.ProductCode), cancellationToken), cancellationToken);

        if (entities is null || entities.Count == 0)
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

        var result = await MappingProductDetailInternalAsync(product.ToDetail(), cancellationToken);
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

        var result = await MappingProductDetailInternalAsync(entity.ToDetail(), cancellationToken);
        
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
    private async Task<List<ProductSummary>> GetProductSummariesInternalAsync(IEnumerable<Product>? entities, CancellationToken cancellationToken)
    {
        if (entities is null)
        {
            return [ ];
        }
        
        var categoryIds = entities.Select(x => x.CategoryId).Distinct();
        var subCategoryIds = entities.Select(x => x.SubCategoryId).Distinct();

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

    private async Task<List<ProductDetail>> MappingProductDetailsInternalAsync(List<Product> entities, CancellationToken cancellationToken)
    {
        var categoryIds = entities.Select(x => x.CategoryId).Distinct();
        var subCategoryIds = entities.Select(x => x.SubCategoryId).Distinct();
        
        var categories = await _categoryRepository.GetEntitiesQueryAsync(x => categoryIds.Contains(x.Id), cancellationToken);
        var subCategories = await _subCategoryRepository.GetEntitiesQueryAsync(x => subCategoryIds.Contains(x.Id), cancellationToken);
        
        var listDetails = new List<ProductDetail>();

        foreach (var entity in entities)
        {
            var detail = entity.ToDetail();
            
            var category = categories.FirstOrDefault(x => x.Id == entity.CategoryId);
            var subCategory = subCategories.FirstOrDefault(x => x.Id == entity.SubCategoryId);
            
            detail.Category = category is null ? string.Empty : category.Name;
            detail.SubCategory = subCategory is null ? string.Empty : subCategory.Name;

            listDetails.Add(detail);
        }

        return listDetails;
    }

    private async Task<ProductDetail> MappingProductDetailInternalAsync(ProductDetail detail, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == detail.CategoryId, cancellationToken);
        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == detail.SubCategoryId, cancellationToken);

        detail.Category = category is null ? string.Empty : category.Name;
        detail.SubCategory = subCategory is null ? string.Empty : subCategory.Name;

        return detail;
    }
    #endregion
}