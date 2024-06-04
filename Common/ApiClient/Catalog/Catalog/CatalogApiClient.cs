using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;
using Platform.Common.Session;

namespace ApiClient.Catalog.Catalog;

public interface ICatalogApiClient
{
    Task<ApiStatusResult> ValidateCatalogCodeAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken = default);
}

public class CatalogApiClient : CommonApiClient, ICatalogApiClient
{
    public CatalogApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState) 
        : base(httpClientFactory, configuration, sessionState)
    {
    }

    public async Task<ApiStatusResult> ValidateCatalogCodeAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.ValidateCatalogCode}";

        url = url.AddQueryStringParameter(nameof(catalogCode), catalogCode)
            .AddQueryStringParameter(nameof(type), ((int)type).ToString());

        var result = await GetStatusAsync(url, cancellationToken);

        return result;
    }
}