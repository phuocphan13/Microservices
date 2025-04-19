using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;
using Platform.Database.ElasticSearch;

namespace Logging.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class LogController : ApiController
{
    private readonly ElasticFactory _elasticFactory;
    
    public LogController(ElasticFactory elasticFactory, ILogger<ApiController> logger) : base(logger)
    {
        _elasticFactory = elasticFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        //await _elasticFactory.GetAllByIndexAsync(cancellationToken);
        return Ok();
    }
}