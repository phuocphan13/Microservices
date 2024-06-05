using ApiClient.Catalog;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.DirectApiClients.Catalog;

public interface IProductInternalClient
{
    Task<ApiCollectionResult<ProductSummary>> GetProductsByListCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default);
}

public class ProductInternalClient : CommonApiClient, IProductInternalClient
{
    private readonly string _baseUrl;

    public ProductInternalClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
        _baseUrl = GetServiceUrl(ServiceConstants.Api.Catalog);
    }

    public async Task<ApiCollectionResult<ProductSummary>> GetProductsByListCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken)
    {
        var url = $"{_baseUrl}{ApiUrlConstants.GetProductsByListCodes}";

        foreach (var code in codes)
        {
            url = url.AddQueryStringParameter(nameof(codes), code);
        }

        var result = await GetCollectionAsync<ProductSummary>(url, cancellationToken);

        return result;
    }
}