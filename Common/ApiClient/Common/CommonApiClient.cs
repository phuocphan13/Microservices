using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Platform.ApiBuilder;
using Platform.ApiBuilder.Helpers;
using Platform.Common.Session;

namespace ApiClient.Common;

public class CommonApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ISessionState _sessionState;

    protected CommonApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ISessionState sessionState)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _sessionState = sessionState;
    }

    protected string GetIdentityServerBaseUrl()
    {
        return _configuration["ApiServices:IdentityServiceApi"];
    }

    protected string GetBaseUrl()
    {
        return _configuration["ApiServices:OcelotApiGw"];
    }

    protected string GetServiceUrl(string serviceName)
    {
        string url = serviceName switch
        {
            ServiceConstants.Api.Catalog => _configuration[$"Microservices:{ServiceConstants.ApiAppSettting.Catalog}"],
            ServiceConstants.Api.Discount => _configuration[$"Microservices:{ServiceConstants.ApiAppSettting.Discount}"],
            ServiceConstants.Api.Identity => _configuration[$"Microservices:{ServiceConstants.ApiAppSettting.Identity}"],
            _ => string.Empty
        };

        return url + "/api/v1";
    }

    protected async Task<ApiStatusResult> GetStatusAsync(string url, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return TransferResponseMessageToStatusAsync(httpResponseMessage);
    }

    protected async Task<ApiDataResult<TResult>> GetSingleAsync<TResult>(string url, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiDataResult<TResult>>(httpResponseMessage, cancellationToken);

        return result;
    }

    protected async Task<ApiCollectionResult<TResult>> GetCollectionAsync<TResult>(string url, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        var result = await HttpResponseHelpers.TransformResponseToData<ApiCollectionResult<TResult>>(httpResponseMessage, cancellationToken);

        return result;
    }

    protected async Task<ApiDataResult<TResult>> PostAsync<TResult>(string url, object requestBody, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await HttpResponseHelpers.TransformResponseToData<ApiDataResult<TResult>>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiStatusResult> PostAsync(string url, object requestBody, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
        
        return TransferResponseMessageToStatusAsync(httpResponseMessage);
    }

    protected async Task<ApiDataResult<TResult>> PutAsync<TResult>(string url, object requestBody, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = HttpClientBuilder();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await HttpResponseHelpers.TransformResponseToData<ApiDataResult<TResult>>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiStatusResult> DeleteAsync(string url, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        var httpClient = HttpClientBuilder();
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

    private HttpClient HttpClientBuilder()
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        var token = _sessionState.GetAccessToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
        }

        return httpClient;
    }
}