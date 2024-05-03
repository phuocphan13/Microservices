using ApiClient.Common;
using Microsoft.Extensions.Configuration;
using Platform.ApiBuilder;

namespace ApiClient.Catalog.Validation;

public interface IValidationApiClient
{
    Task<ApiStatusResult> ValidateCatalogCodeAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken = default);
}

public class ValidationApiClient : CommonApiClient, IValidationApiClient
{
    public ValidationApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        : base(httpClientFactory, configuration)
    {
    }

    public async Task<ApiStatusResult> ValidateCatalogCodeAsync(string catalogCode, DiscountEnum type, CancellationToken cancellationToken)
    {
        var url = $"{GetBaseUrl()}{ApiUrlConstants.ValidateCatalogCode}";

        url = url.AddQueryStringParameter(nameof(catalogCode), catalogCode, true)
            .AddQueryStringParameter(nameof(type), ((int)type).ToString());

        var result = await GetAsync<ApiStatusResult>(url, cancellationToken);

        return result;
    }
}