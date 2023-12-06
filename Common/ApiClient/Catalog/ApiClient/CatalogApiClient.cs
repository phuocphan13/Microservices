using ApiClient.Catalog.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Catalog.ApiClient;

public interface ICatalogApiClient
{
    Task<ApiStatusResult<List<ProductSummary>>> GetProducts(CancellationToken cancellationToken = default);
    Task<ApiStatusResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApiStatusResult<List<ProductSummary>>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken = default);
}

public class CatalogApiClient : CommonApiClient, ICatalogApiClient
{
    public CatalogApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiStatusResult<List<ProductSummary>>> GetProducts(CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProducts}";

        var result = await GetAsync<List<ProductSummary>>(url, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult<ProductDetail>> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductById}";

        url = url.AddQueryStringParameter("id", id, true);

        var result = await GetAsync<ProductDetail>(url, cancellationToken);

        return result;
    }

    public async Task<ApiStatusResult<List<ProductSummary>>> GetProductByCategoryAsync(string category, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.GetProductByCategory}";

        url = url.AddDataInUrl(nameof(category), category);

        var result = await GetAsync<List<ProductSummary>>(url, cancellationToken);
        
        return result;
    }

}