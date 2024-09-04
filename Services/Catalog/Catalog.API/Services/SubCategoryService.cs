using ApiClient.Catalog.SubCategory.Models;
using Catalog.API.Extensions;
using Catalog.API.Repositories;
using Catalog.API.Services.Caches;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services;

public interface ISubCategoryService
{
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryBySeachAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
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

    public async Task<SubCategoryDetail?> GetSubCategoryBySeachAsync(string search, PropertyName propertyName, CancellationToken cancellationToken)
    {
        SubCategory? data = propertyName switch
        {
            PropertyName.Id => await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == search, cancellationToken),
            PropertyName.Name => await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == search, cancellationToken),
            PropertyName.Code => await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.SubCategoryCode == search, cancellationToken),
            _ => null
        };

        if (data is null)
        {
            return null;
        }

        return data.ToDetail();
    }

    public async Task<List<SubCategorySummary>> GetSubCategoriesAsync(CancellationToken cancellationToken)
    {
        var entities = await _cachedService.GetCachedSubCategoriesAsync(cancellationToken);

        if (entities is null)
        {
            return new();
        }
        
        return entities.Select(x => x.ToSummaryFromCachedModel()).ToList();
    }

    public async Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken)
    {
        var entities = await _subCategoryRepository.GetEntitiesQueryAsync(x => x.CategoryId.Contains(categoryId), cancellationToken);
        
        if (entities is null)
        {
            return new();
        }

        return entities.Select(x => x.ToSummary()).ToList();
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
        
        //Todo: Move this logic to Controller
        // var isExisted = await _subCategoryRepository.AnyAsync(x => (x.Name == body.Name || x.SubCategoryCode == body.SubCategoryCode) && x.Id != body.Id, cancellationToken);
        // if (isExisted)
        // {
        //     apiDataResult.Message = ResponseMessages.SubCategory.SubCategoryExisted;
        //     return apiDataResult;
        // }

        subCategory.ToUpdateSubCategory(body);

        var result = await _subCategoryRepository.UpdateEntityAsync(subCategory, cancellationToken);

        if (!result)
        {
            return null;
        }

        return subCategory.ToDetail();
    }
}