using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Repositories;

namespace Catalog.API.Services;

public interface ICategoryService
{
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<CategoryDetail?> GetCategoryBySeachAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<CategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<CategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        bool result = propertyName switch
        {
            PropertyName.Id => await _categoryRepository.AnyAsync(x => x.Id == search, cancellationToken),
            PropertyName.Name => await _categoryRepository.AnyAsync(x => x.Name == search, cancellationToken),
            PropertyName.Code => await _categoryRepository.AnyAsync(x => x.CategoryCode == search, cancellationToken),
            _ => false
        };

        return result;
    }

    public async Task<CategoryDetail?> GetCategoryBySeachAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        Category? data = propertyName switch
        {
            PropertyName.Id => await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == search, cancellationToken),
            PropertyName.Name => await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == search, cancellationToken),
            PropertyName.Code => await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.CategoryCode == search, cancellationToken),
            _ => null
        };

        if (data is null)
        {
            return null;
        }

        return data.ToDetail();
    }

    public async Task<List<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var entities = await _categoryRepository.GetEntitiesAsync(cancellationToken);

        return entities.Select(x => x.ToSummary()).ToList();
    }

    public async Task<CategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var category = requestBody.ToCreateCategory();

        category = await _categoryRepository.CreateEntityAsync(category, cancellationToken);

        if (string.IsNullOrWhiteSpace(category.Id))
        {
            return null;
        }
        
        return category.ToDetail();
    }

    public async Task<CategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == requestBody.Id, cancellationToken);

        category.ToUpdateCategory(requestBody);

        var updateItem = await _categoryRepository.UpdateEntityAsync(category, cancellationToken);

        if (!updateItem)
        {
            return null;
        }
        
        return category.ToDetail();
    }

    public async Task<bool> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.DeleteEntityAsync(id, cancellationToken);

        return result;
    }
}