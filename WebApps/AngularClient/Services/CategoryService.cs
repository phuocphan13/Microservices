using ApiClient.Catalog.Category;
using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;

namespace AngularClient.Services;

public interface ICategoryService
{
    Task<List<CategorySummary>?> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<SubCategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
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

    public async Task<SubCategoryDetail?> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByIdAsync(id, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<SubCategoryDetail?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.GetCategoryByNameAsync(name, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<SubCategoryDetail?> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryApiClient.CreateCategoryAsync(requestBody, cancellationToken);

        if (result.IsSuccessCode && result.Data is not null)
        {
            return result.Data;
        }

        return null;
    }

    public async Task<SubCategoryDetail?> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
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
