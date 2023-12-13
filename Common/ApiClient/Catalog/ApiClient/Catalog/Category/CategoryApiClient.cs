using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Models.Catalog.Product;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Catalog.ApiClient.Catalog.Category;

public interface ICategoryApiClient
{
    Task<ApiDataResult<List<CategorySummary>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<ApiDataResult<CategorySummary>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken = default);
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

    public async Task<ApiDataResult<CategorySummary>> GetCategoryByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryById}";

        url = url.AddQueryStringParameter("id", id, true);

        var result = await GetAsync<CategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategorySummary>> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetCategoryByName}";

        url = url.AddDataInUrl(nameof(name), name);

        var result = await GetAsync<CategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategorySummary>> CreateCategoryAsync(CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateCategory}";

        var result = await PostAsync<CreateCategoryRequestBody, CategorySummary>(url,requestBody,cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<CategorySummary>> UpdateCategoryAsync(UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateCategory}";

        var result = await PutAsync<UpdateCategoryRequestBody, CategorySummary>(url, requestBody, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult> DeleteCategoryAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.DeleteCategory}";

        var result = await DeleteAsync(url, cancellationToken);

        return result;
    }
}
