using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Common;

public class CommonApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    protected CommonApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected string GetBaseUrl()
    {
        return _configuration["ApiServices:OcelotApiGw"];
    }

    protected async Task<ApiStatusResult<TResult>> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        ApiStatusResult<TResult> result = new()
        {
            IsSuccessCode = httpResponseMessage.IsSuccessStatusCode,
        };

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            result.Data = await JsonSerializer.DeserializeAsync<TResult>(contentStream, options, cancellationToken: cancellationToken);

            return result;
        }

        result.InternalErrorCode = (int)httpResponseMessage.StatusCode;

        return result;
    }
}