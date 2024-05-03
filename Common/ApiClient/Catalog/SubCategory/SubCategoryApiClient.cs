using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;

namespace ApiClient.Catalog.SubCategory;

public interface ISubCategoryApiClient
{
    Task<ApiCollectionResult<SubCategorySummary>> GetSubCategories(CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByName(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> GetSubCategoryById(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoriesByCategoryId(string categoryId, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteSubCategory(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> CreateSubCategory(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> UpdateSubCategory(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
}

public class SubCategoryApiClient : CommonApiClient, ISubCategoryApiClient
{
    public SubCategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiCollectionResult<SubCategorySummary>> GetSubCategories(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategories}";

        var result = await GetCollectionAsync<SubCategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByName(string name, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategoryByName}";
        url = url.AddDataInUrl(nameof(name), name);

        var result = await GetAsync<SubCategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> GetSubCategoryById(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategoryById}";
        url = url.AddDataInUrl(nameof(id), id);

        var result = await GetAsync<SubCategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<List<SubCategorySummary>>> GetSubCategoriesByCategoryId(string categoryId, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategoryByCategoryId}";
        url = url.AddDataInUrl(nameof(categoryId), categoryId);

        var result = await GetAsync<List<SubCategorySummary>>(url, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult> DeleteSubCategory(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.DeleteSubCategory}";

        url = url.AddDataInUrl(nameof(id), id);

        var result = await DeleteAsync(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> CreateSubCategory(CreateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.CreateSubCategory}";

        var result = await PostAsync<CreateSubCategoryRequestBody, SubCategorySummary>(url, body, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> UpdateSubCategory(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateSubCategory}";

        var result = await PutAsync<UpdateSubCategoryRequestBody, SubCategorySummary>(url, body, cancellationToken);

        return result;
    }
}