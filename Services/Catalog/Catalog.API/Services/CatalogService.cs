using Catalog.API.Entities;
using Platform.ApiBuilder;
using Platform.Database.MongoDb;

namespace Catalog.API.Services;

public interface ICatalogService
{
    Task<ApiStatusResult> ValidateCatalogCodeAsync(string? catalogCode, DiscountEnum type, CancellationToken cancellationToken = default);
}

public class CatalogService : ICatalogService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;
    private readonly IRepository<Product> _productRepository;

    public CatalogService(
        IRepository<Category> categoryRepository,
        IRepository<SubCategory> subCategoryRepository,
        IRepository<Product> productRepository)
    {
        ArgumentNullException.ThrowIfNull(categoryRepository);
        ArgumentNullException.ThrowIfNull(categoryRepository);
        ArgumentNullException.ThrowIfNull(productRepository);
        
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _productRepository = productRepository;
    }
    
    public async Task<ApiStatusResult> ValidateCatalogCodeAsync(string? catalogCode, DiscountEnum type, CancellationToken cancellationToken)
    {
        var isExisted = false;
        switch (type)
        {
            case DiscountEnum.Category:
            {
                isExisted = await _categoryRepository.AnyAsync(x => x.CategoryCode == catalogCode, cancellationToken);
                break;
            }
            case DiscountEnum.SubCategory:
            {
                isExisted = await _subCategoryRepository.AnyAsync(x => x.SubCategoryCode == catalogCode, cancellationToken);
                break;
            }
            case DiscountEnum.Product:
            {
                isExisted = await _productRepository.AnyAsync(x => x.ProductCode == catalogCode, cancellationToken);
                break;
            }
            case DiscountEnum.All:
                break;
            case DiscountEnum.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return new ApiStatusResult()
        {
            Message = isExisted ? string.Empty : $"{type.ToString()} with code: '{catalogCode}' is not existed."
        };
    }
}