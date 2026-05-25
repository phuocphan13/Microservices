using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common.Models.Paging;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Platform.Database.MongoDb;
using SubCategory = Catalog.API.Entities.SubCategory;

namespace Catalog.API.Services;

public interface ISubCategoryService
{
    // Retrieve operations
    Task<List<SubCategorySummary>> GetAllSubCategoriesAsync(CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetSubCategoryBySearchAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<PagingCollection<SubCategorySummary>> GetPagingSubCategoriesAsync(PagingInfo pagingInfo, CancellationToken cancellationToken = default);

    // Create/Update/Delete operations
    Task<SubCategoryDetail?> CreateSubCategoryAsync(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);

    // Validation operations
    Task<bool> CheckExistingAsync(string search, PropertyName propertyName, CancellationToken cancellationToken = default);
}

public class SubCategoryService : ISubCategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<SubCategory> _subCategoryRepository;
    public SubCategoryService(
        IRepository<SubCategory> subCategoryRepository,
        IRepository<Category> categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Retrieves all subcategories with their associated category names.
    /// </summary>
    public async Task<List<SubCategorySummary>> GetAllSubCategoriesAsync(CancellationToken cancellationToken)
    {
        var subCategoryEntities = await _subCategoryRepository.GetEntitiesAsync(cancellationToken);

        if (subCategoryEntities.Count == 0)
            return [];

        var categoryIds = subCategoryEntities.Select(x => x.CategoryId).Distinct().ToList();
        var categories = await _categoryRepository.GetEntitiesQueryAsync(
            x => categoryIds.Contains(x.Id), cancellationToken);

        var subCategorySummaries = subCategoryEntities
            .Select(x => x.ToSummary())
            .ToList();

        // Enrich summaries with category names
        foreach (var summary in subCategorySummaries)
        {
            var category = categories.FirstOrDefault(x => x.Id == summary.CategoryId);
            if (category is not null)
                summary.CategoryName = category.Name;
        }

        return subCategorySummaries;
    }

    /// <summary>
    /// Retrieves a subcategory by ID from the cache.
    /// </summary>
    public async Task<SubCategoryDetail?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return null;
        }
        
        return entity.ToDetail();
    }

    /// <summary>
    /// Retrieves a subcategory by name from the cache.
    /// </summary>
    public async Task<SubCategoryDetail?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        
        if (entity is null)
        {
            return null;
        }

        return entity.ToDetail();
    }

    /// <summary>
    /// Retrieves a subcategory using a dynamic search property (Id, Name, or Code).
    /// </summary>
    public async Task<SubCategoryDetail?> GetSubCategoryBySearchAsync(
        string search,
        PropertyName propertyName,
        CancellationToken cancellationToken)
    {
        SubCategory? entity = null;
        if (propertyName == PropertyName.Id)
        {
            entity = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Id == search, cancellationToken);
        }

        if (propertyName == PropertyName.Name)
        {
            entity = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.Name == search, cancellationToken);
        }

        if (propertyName == PropertyName.Code)
        {
            entity = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(x => x.SubCategoryCode == search, cancellationToken);
        }

        if (entity is null)
        {
            return null;
        }

        return entity.ToDetail();
    }

    /// <summary>
    /// Retrieves all subcategories belonging to a specific category.
    /// </summary>
    public async Task<List<SubCategorySummary>> GetSubCategoriesByCategoryIdAsync(
        string categoryId,
        CancellationToken cancellationToken)
    {
        var entities = await _subCategoryRepository.GetEntitiesQueryAsync(
            x => !string.IsNullOrWhiteSpace(x.CategoryId) && x.CategoryId.Contains(categoryId),
            cancellationToken);

        return entities.Select(x => x.ToSummary()).ToList() ?? [];
    }

    /// <summary>
    /// Retrieves a paginated collection of subcategories.
    /// </summary>
    public async Task<PagingCollection<SubCategorySummary>> GetPagingSubCategoriesAsync(
        PagingInfo pagingInfo,
        CancellationToken cancellationToken)
    {
        var entities = await GetAllSubCategoriesAsync(cancellationToken);
        var skipCount = pagingInfo.Start ?? 0;
        var takeCount = pagingInfo.Length ?? 10;

        var pagedItems = entities
            .Skip(skipCount)
            .Take(takeCount)
            .ToList();

        return new PagingCollection<SubCategorySummary>(pagedItems);
    }

    /// <summary>
    /// Creates a new subcategory.
    /// </summary>
    public async Task<SubCategoryDetail?> CreateSubCategoryAsync(
        CreateSubCategoryRequestBody requestBody,
        CancellationToken cancellationToken)
    {
        var subcategory = requestBody.ToCreateSubCategory();
        await _subCategoryRepository.CreateEntityAsync(subcategory, cancellationToken);

        return string.IsNullOrWhiteSpace(subcategory.Id) ? null : subcategory.ToDetail();
    }

    /// <summary>
    /// Updates an existing subcategory.
    /// </summary>
    public async Task<SubCategoryDetail?> UpdateSubCategoryAsync(
        UpdateSubCategoryRequestBody body,
        CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetEntityFirstOrDefaultAsync(
            x => x.Id == body.Id, cancellationToken);

        if (subCategory is null)
            return null;

        subCategory.ToUpdateSubCategory(body);
        var result = await _subCategoryRepository.UpdateEntityAsync(subCategory, cancellationToken);

        return result ? subCategory.ToDetail() : null;
    }

    /// <summary>
    /// Deletes a subcategory by ID.
    /// </summary>
    public async Task<bool> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken)
    {
        return await _subCategoryRepository.DeleteEntityAsync(id, cancellationToken);
    }

    /// <summary>
    /// Checks if a subcategory exists based on a search property.
    /// </summary>
    public async Task<bool> CheckExistingAsync(
        string search,
        PropertyName propertyName,
        CancellationToken cancellationToken)
    {
        return propertyName switch
        {
            PropertyName.Id => await _subCategoryRepository.AnyAsync(
                x => x.Id == search, cancellationToken),
            PropertyName.Name => await _subCategoryRepository.AnyAsync(
                x => x.Name == search, cancellationToken),
            PropertyName.Code => await _subCategoryRepository.AnyAsync(
                x => x.SubCategoryCode == search, cancellationToken),
            _ => false
        };
    }
}