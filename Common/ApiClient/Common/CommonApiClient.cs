using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Platform.ApiBuilder;
using Platform.ApiBuilder.Helpers;

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

    protected async Task<ApiDataResult<TResult>> GetSingleAsync<TResult>(string url, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        var result = await HttpResponseHelpers.TransformResponseToData<TResult>(httpResponseMessage, cancellationToken);

        return new ApiDataResult<TResult>(result);
    }

    protected async Task<ApiCollectionResult<TResult>> GetCollectionAsync<TResult>(string url, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        var result = await HttpResponseHelpers.TransformResponseToData<List<TResult>>(httpResponseMessage, cancellationToken);

        return new ApiCollectionResult<TResult>(result);
    }

    protected async Task<ApiDataResult<TResult>> PostAsync<TResult>(string url, object requestBody, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await HttpResponseHelpers.TransformResponseToData<ApiDataResult<TResult>>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiDataResult<TResult>> PutAsync<TResult>(string url, object requestBody, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await HttpResponseHelpers.TransformResponseToData<ApiDataResult<TResult>>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiStatusResult> DeleteAsync(string url, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return TransferResponseMessageToStatusAsync(httpResponseMessage);
    }

    private ApiStatusResult TransferResponseMessageToStatusAsync(HttpResponseMessage? httpResponseMessage)
    {
        var result = new ApiStatusResult();
        
        if (httpResponseMessage is null)
        {
            result.InternalErrorCode = -1;

            return result;
        }

        return result;
    }
}