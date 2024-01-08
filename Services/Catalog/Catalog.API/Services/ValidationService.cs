using ApiClient.Common;
using Catalog.API.Entities;
using Catalog.API.Repositories;

namespace Catalog.API.Services;

public interface IValidationService
{
    Task<ApiStatusResult> ValidateCatalogCodeAsync(string? catalogCode, DiscountEnum type, CancellationToken cancellationToken = default);
}

public class ValidationService : IValidationService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;
    private readonly IRepository<Product> _productRepository;

    public ValidationService(
        IRepository<Category> categoryRepository,
        IRepository<SubCategory> subCategoryRepository,
        IRepository<Product> productRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
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
        }

        return new ApiStatusResult()
        {
            Message = isExisted ? string.Empty : $"{type.ToString()} with code: '{catalogCode}' is not existed."
        };
    }
}