using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Catalog.Category;

public interface ICategoryApiClient
{
    Task<ApiDataResult<List<CategorySummary>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategoryDetail>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategoryDetail>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategoryDetail>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategoryDetail>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}
public class CategoryApiClient : CommonApiClient, ICategoryApiClient
{
    public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiDataResult<List<CategorySummary>>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategories}";

        var result = await GetAsync<List<CategorySummary>>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategoryDetail>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryById}";
        url = url.AddDataInUrl(nameof(id), id);

        var result = await GetAsync<SubCategoryDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategoryDetail>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryByName}";

        url = url.AddDataInUrl(nameof(name), name);

        var result = await GetAsync<SubCategoryDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategoryDetail>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateCategory}";

        var result = await PostAsync<CreateCategoryRequestBody, SubCategoryDetail>(url,requestBody,cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategoryDetail>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateCategory}";

        var result = await PutAsync<UpdateCategoryRequestBody, SubCategoryDetail>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.DeleteCategory}";

        url = url.AddDataInUrl(nameof(id), id);

        var result = await DeleteAsync(url, cancellationToken);

        return result;
    }
}
