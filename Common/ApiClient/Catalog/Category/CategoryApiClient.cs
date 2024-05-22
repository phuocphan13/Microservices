using ApiClient.Catalog.Category.Models;
using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.Catalog.Category;

public interface ICategoryApiClient
{
    Task<ApiCollectionResult<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategoryDetail>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategoryDetail>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategoryDetail>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategoryDetail>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
}
public class CategoryApiClient : CommonApiClient, ICategoryApiClient
{
    public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
    : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiCollectionResult<CategorySummary>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategories}";

        var result = await GetCollectionAsync<CategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategoryDetail>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryById}";
        url = url.AddDataInUrl(nameof(id), id);

        var result = await GetSingleAsync<CategoryDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategoryDetail>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryByName}";

        url = url.AddDataInUrl(nameof(name), name);

        var result = await GetSingleAsync<CategoryDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategoryDetail>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateCategory}";

        var result = await PostAsync<CategoryDetail>(url,requestBody,cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategoryDetail>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateCategory}";

        var result = await PutAsync<CategoryDetail>(url, requestBody, cancellationToken);

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
