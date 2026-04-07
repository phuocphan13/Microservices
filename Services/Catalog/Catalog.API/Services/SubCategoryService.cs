using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common.Models.Paging;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Models;
using Catalog.API.Services.Caches;
using Platform.Database.MongoDb;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services;

public interface ISubCategoryService
{
    Task<List<SubCategorySummary>> GetAllSubCategoriesAsync(CancellationToken cancellationToken = default);
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<PagingCollection<SubCategorySummary>> GetPagingSubCategoriesAsync(PagingInfo pagingInfo, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesFromCachedAsync(CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> CreateSubCategoryAsync(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
}

public class SubCategoryService : ISubCategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;
    
    private readonly ISubCategoryCachedService _cachedService;

    public SubCategoryService(IRepository<SubCategory> subCategoryRepository, ISubCategoryCachedService cachedService, IRepository<Category> categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
        _cachedService = cachedService ??throw new ArgumentNullException(nameof(cachedService));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    public async Task<List<SubCategorySummary>> GetAllSubCategoriesAsync(CancellationToken cancellationToken)
    {
        var subCategoryEntities = await _subCategoryRepository.GetEntitiesAsync(cancellationToken);

        if (subCategoryEntities is null || subCategoryEntities.Count == 0)
        {
            return [ ];
        }

        var categoryIds = subCategoryEntities.Select(x => x.CategoryId).Distinct().ToList();
        var categories = await _categoryRepository.GetEntitiesQueryAsync(x => categoryIds.Contains(x.Id), cancellationToken);

        var subCategorySummaries = subCategoryEntities.Select(x => x.ToSummary()).ToList();

        foreach (var sub in subCategorySummaries)
        {
            var category = categories.FirstOrDefault(x => x.Id == sub.CategoryId);

            if (category is not null)
            {
                sub.CategoryName = category.Name;
            }
        }

        return subCategorySummaries;
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

    //So 5
    public async Task<PagingCollection<SubCategorySummary>> GetPagingSubCategoriesAsync(PagingInfo pagingInfo, CancellationToken cancellationToken)
    {
        var entities = await GetAllSubCategoriesAsync(cancellationToken);

        var pagingCollection = new List<SubCategorySummary>(entities.Skip(pagingInfo.Start ?? 0).Take(pagingInfo.Length ?? 10));
        
        return new PagingCollection<SubCategorySummary>(pagingCollection);
    }

    public async Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _subCategoryRepository.DeleteEntityAsync(id, cancellationToken);

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

    public async Task<List<SubCategorySummary>> GetSubCategoriesFromCachedAsync(CancellationToken cancellationToken)
    {
        var entities = await _cachedService.GetCachedSubCategoriesAsync(cancellationToken);

        if (entities is null)
        {
            return [];
        }
        
        return entities.Select(x => x.ToSummaryFromCachedModel("","")).ToList();
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
}