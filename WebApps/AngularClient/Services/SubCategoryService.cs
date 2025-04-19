using ApiClient.Catalog.SubCategory;
using ApiClient.Catalog.SubCategory.Models;
using Platform.ApiBuilder;


namespace AngularClient.Services;

public interface ISubCategoryService
{
    Task<List<SubCategorySummary>?> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
    Task<SubCategorySummary?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<SubCategorySummary?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<List<SubCategorySummary>?> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<ApiStatusResult?> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategorySummary?> CreateSubCategoryAsync(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<SubCategorySummary?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
}

public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryApiClient _subCategoryApiClient;

    public SubCategoryService(ISubCategoryApiClient subCategoryApiClient)
    {
        _subCategoryApiClient = subCategoryApiClient;
    }
    //Số 2
    public async Task<List<SubCategorySummary>?> GetSubCategoriesAsync(CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.GetSubCategories(cancellationToken);

        if (result is not null && result.IsSuccessStatusCode)
        {
            if (result.IsSuccessStatusCode && result.Result is not null)
            {
                return result.Result.ToList();
            }
        }

        return null;
    }

    public async Task<SubCategorySummary?> GetSubCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.GetSubCategoryByName(name, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<SubCategorySummary?> GetSubCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.GetSubCategoryById(id, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<List<SubCategorySummary>?> GetSubCategoriesByCategoryIdAsync(string categoryId, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.GetSubCategoriesByCategoryId(categoryId, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result != null)
        {
            return result.Result.ToList();
        }

        return null;
    }

    public async Task<ApiStatusResult?> DeleteSubCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.DeleteSubCategory(id, cancellationToken);

        return result;
    }

    public async Task<SubCategorySummary?> CreateSubCategoryAsync(CreateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.CreateSubCategory(body, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<SubCategorySummary?> UpdateSubCategoryAsync(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var result = await _subCategoryApiClient.UpdateSubCategory(body, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result != null)
        {
            return result.Result;
        }

        return null;
    }
}