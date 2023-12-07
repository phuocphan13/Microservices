using System.Net;
using System.Text.Json;
using ApiClient.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

    protected async Task<ApiDataResult<TResult>> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
        
        return await TransferResponseMessageToDataAsync<TResult>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiDataResult<TResult>> PostAsync<TRequestBody, TResult>(string url, TRequestBody requestBody, CancellationToken cancellationToken)
        where TRequestBody : BaseRequestBody, new()
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await TransferResponseMessageToDataAsync<TResult>(httpResponseMessage, cancellationToken);
    }

    protected async Task<ApiDataResult<TResult>> PutAsync<TRequestBody, TResult>(string url, TRequestBody requestBody, CancellationToken cancellationToken)
        where TRequestBody : BaseRequestBody
        where TResult : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await TransferResponseMessageToDataAsync<TResult>(httpResponseMessage, cancellationToken);
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
            result.HttpErrorCode = -1;

            return result;
        }

        return result;
    }

    private async Task<ApiDataResult<TResult>> TransferResponseMessageToDataAsync<TResult>(HttpResponseMessage? httpResponseMessage, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        var result = new ApiDataResult<TResult>();
        if (httpResponseMessage is null)
        {
            //Todo
            result.HttpErrorCode = -1;

            return result;
        }

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);
            
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            
            var deserialize = await JsonSerializer.DeserializeAsync<ApiDataResult<TResult>>(contentStream, options, cancellationToken: cancellationToken);
            
            if (deserialize is not null)
            {
                result = deserialize;
            }
            
            return result;
        }

        result.HttpErrorCode = (int)httpResponseMessage.StatusCode;

        return result;
    }
}