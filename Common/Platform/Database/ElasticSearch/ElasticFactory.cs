using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Platform.Configurations.Options;
using Platform.Database.ElasticSearch.Model;

namespace Platform.Database.ElasticSearch;

public class ElasticFactory
{
    private readonly ElasticsearchClient _client;
    private readonly LogElasticSearchDbOptions _options;

    public ElasticFactory(IOptions<LogElasticSearchDbOptions> elasticSearchOptions)
    {
        ArgumentNullException.ThrowIfNull(elasticSearchOptions);

        _options = elasticSearchOptions.Value;

        var settings = new ElasticsearchClientSettings(new Uri(_options.Endpoint)) 
            .DefaultIndex(_options.Index)
            .DefaultFieldNameInferrer(s => s);

        _client = new ElasticsearchClient(settings);
    }
    
    public ElasticsearchClient GetClient()
    {
        return _client;
    }

    public async Task<IReadOnlyCollection<ElasticResponseModel>> GetAllLogsAsync(CancellationToken cancellationToken)
    {
        var result = await _client.SearchAsync<ElasticResponseModel>(s => s
                .Index(_options.Index)
                .Query(q => q.MatchAll(_ => {})), cancellationToken);

        return result.Documents;
    }
}