using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.Catalog.SubCategory;

public interface ISubCategoryApiClient
{
    Task<ApiCollectionResult<SubCategorySummary>> GetSubCategories(CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> GetSubCategoryByName(string name, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> GetSubCategoryById(string id, CancellationToken cancellationToken = default);
    Task<ApiCollectionResult<SubCategorySummary>> GetSubCategoriesByCategoryId(string categoryId, CancellationToken cancellationToken = default);
    Task<ApiStatusResult> DeleteSubCategory(string id, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> CreateSubCategory(CreateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
    Task<ApiDataResult<SubCategorySummary>> UpdateSubCategory(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken = default);
}

public class SubCategoryApiClient : CommonApiClient, ISubCategoryApiClient
{
    public SubCategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
        : base(httpClientFactory, configuration, sessionState)
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

        var result = await GetSingleAsync<SubCategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> GetSubCategoryById(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategoryById}";
        url = url.AddDataInUrl(nameof(id), id);

        var result = await GetSingleAsync<SubCategorySummary>(url, cancellationToken);

        return result;
    }

    public async Task<ApiCollectionResult<SubCategorySummary>> GetSubCategoriesByCategoryId(string categoryId, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetSubCategoryByCategoryId}";
        url = url.AddDataInUrl(nameof(categoryId), categoryId);

        var result = await GetCollectionAsync<SubCategorySummary>(url, cancellationToken);

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

        var result = await PostAsync<SubCategorySummary>(url, body, cancellationToken);

        return result;
    }

    public async Task<ApiDataResult<SubCategorySummary>> UpdateSubCategory(UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.UpdateSubCategory}";

        var result = await PutAsync<SubCategorySummary>(url, body, cancellationToken);

        return result;
    }
}