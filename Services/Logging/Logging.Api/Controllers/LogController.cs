using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace Logging.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class LogController : ApiController
{
    public LogController(ILogger<ApiController> logger) : base(logger)
    {
    }
}