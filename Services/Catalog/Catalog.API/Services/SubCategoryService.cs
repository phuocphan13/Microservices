using ApiClient.Catalog.SubCategory.Models;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services.Caches;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services;

public interface ISubCategoryService
{
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> CreateSubCategoryAsync(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
}

public class SubCategoryService : ISubCategoryService
{
    private readonly IRepository<SubCategory> _subCategoryRepository;
    private readonly ISubCategoryCachedService _cachedService;

    public SubCategoryService(IRepository<SubCategory> subCategoryRepository, ISubCategoryCachedService cachedService)
    {
        _subCategoryRepository = subCategoryRepository;
        _cachedService = cachedService;
    }

    public async Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        bool result = propertyName switch
        {
            PropertyName.Id => await _subCategoryRepository.AnyAsync(x => x.Id == search, cancellationToken),
            PropertyName.Name => await _subCategoryRepository.AnyAsync(x => x.Name == search, cancellationToken),
            PropertyName.Code => await _subCategoryRepository.AnyAsync(x => x.SubCategoryCode == search, cancellationToken),
            _ => false
        };

        return result;
    }

    public async Task<SubCategoryDetail?> GetSubCategoryBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        var entity = await _cachedService.GetSubCategoryCachedBySearchAsync(search, propertyName, cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return entity.ToDetailFromCachedModel();
    }

    public async Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken)
    {
        var entities = await _cachedService.GetCachedSubCategoriesAsync(cancellationToken);

        if (entities is null)
        {
            return [];
        }
        
        return entities.Select(x => x.ToSummaryFromCachedModel()).ToList();
    }

    public async Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken)
    {
        var entities = await _subCategoryRepository.GetEntitiesQueryAsync(x => !string.IsNullOrWhiteSpace(x.CategoryId) && x.CategoryId.Contains(categoryId), cancellationToken);
        
        if (entities is null)
        {
            return [];
        }

        return entities.Select(x => x.ToSummary()).ToList();
    }

    public async Task<SubCategoryDetail?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _cachedService.GetCachedSubCategoriesByIdAsync(id, cancellationToken);
        
        if (entity is null) 
        {
            return new();
        }

        return entity.ToDetailFromCachedModel();
    }

    public async Task<SubCategoryDetail?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _cachedService.GetCachedSubCategoriesByNameAsync(name, cancellationToken);
        
        if (entity is null)
        {
            return new();
        }

        return entity.ToDetailFromCachedModel();
    }

    public async Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _subCategoryRepository.DeleteEntityAsync(id, cancellationToken);

        return result;
    }

    public async Task<SubCategoryDetail?> CreateSubCategoryAsync(CreateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var subcategory = requestBody.ToCreateSubCategory();
        await _subCategoryRepository.CreateEntityAsync(subcategory, cancellationToken);

        if (string.IsNullOrWhiteSpace(subcategory.Id))
        {
            return null;
        }

        return subcategory.ToDetail();
    }

    public async Task<SubCategoryDetail?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == body.Id, cancellationToken);

        subCategory.ToUpdateSubCategory(body);

        var result = await _subCategoryRepository.UpdateEntityAsync(subCategory, cancellationToken);

        if (!result)
        {
            return null;
        }

        return subCategory.ToDetail();
    }
}