using System.Text.Json;

namespace Platform.ApiBuilder.Helpers;

public static class HttpResponseHelpers
{
    public static async Task<TResult> TransformResponseToData<TResult>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        TResult result = default!;
        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var deserialize = await JsonSerializer.DeserializeAsync<TResult>(contentStream, options, cancellationToken: cancellationToken);

        if (deserialize is not null)
        {
            result = deserialize;
        }

        return result;
    }
}