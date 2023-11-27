using ApiClient.Catalog.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Catalog.ApiClient;

public interface ICatalogApiClient
{
    Task<ApiStatusResult<List<ProductSummary>>> GetProducts(CancellationToken cancellationToken = default);
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
}