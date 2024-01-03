using System.Text.Json;

namespace Core.Common.Helpers;

public static class HttpResponseHelpers
{
    public static async Task<TResult> TransformResponseToData<TResult>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        where TResult : class, new()
    {
        var result = new TResult();
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