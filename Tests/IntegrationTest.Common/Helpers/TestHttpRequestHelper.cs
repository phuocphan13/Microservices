using Newtonsoft.Json;

namespace IntegrationTest.Common.Helpers;

public static class TestHttpRequestHelper
{
    public static async Task<HttpResponseMessage> PostAsync<T>(T requestBody, HttpClient client, string url)
        where T : class, new()
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
        // Act
        var response = await client.SendAsync(httpRequestMessage, default);

        return response;
    }

    public static async Task<HttpResponseMessage> GetAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url, cancellationToken: default);

        return response;
    }
}