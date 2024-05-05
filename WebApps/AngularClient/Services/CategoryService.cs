using ApiClient.Catalog.Category;
using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Platform.ApiBuilder;

namespace AngularClient.Services;

public interface ICategoryService
{
    Task<List<CategorySummary>?> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<CategoryDetail?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<CategoryDetail?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<CategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<CategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
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

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result.ToList();
        }

        return null;
    }

    public async Task<CategoryDetail?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByIdAsync(id, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<CategoryDetail?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByNameAsync(name, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<CategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.CreateCategoryAsync(requestBody, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<CategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.UpdateCategoryAsync(requestBody, cancellationToken);

        if (result.IsSuccessStatusCode && result.Result is not null)
        {
            return result.Result;
        }

        return null;
    }

    public async Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.DeleteCategoryAsync(id, cancellationToken);

        return result;
    }
}
