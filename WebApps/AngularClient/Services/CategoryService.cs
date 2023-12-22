using ApiClient.Catalog.ApiClient.Catalog.Category;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;

namespace AngularClient.Services;

public interface ICategoryService
{
    Task<List<CategorySummary>?> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<CategorySummary?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<CategorySummary?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<CategorySummary?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<CategorySummary?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}
public class CategoryService : ICategoryService
{
    private readonly ICategoryApiClient _categoryApiClient;
    public CategoryService(ICategoryApiClient categoryApiClient)
    {
        _categoryApiClient = categoryApiClient;
    }
    public async Task<List<CategorySummary>?> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoriesAsync(cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<CategorySummary?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByIdAsync(id, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<CategorySummary?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByNameAsync(name, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<CategorySummary?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.CreateCategoryAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<CategorySummary?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.UpdateCategoryAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.DeleteCategoryAsync(id, cancellationToken);

        return result;
    }
}
