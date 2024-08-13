using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class PermissionController : ApiController
{
    private readonly IPermissionService _permissionService;
    
    public PermissionController(ILogger<ApiController> logger, IPermissionService permissionService) : base(logger)
    {
        _permissionService = permissionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPermissions([FromQuery] string userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("Invalid user id");
        }

        var result = await _permissionService.GetPermissionByUserIdAsync(userId, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> HasPermission([FromQuery] string userId, string feature, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("Invalid user id");
        }

        if (string.IsNullOrWhiteSpace(feature))
        {
            return BadRequest("Invalid feature");
        }

        var result = await _permissionService.HasPermissionAsync(userId, feature, cancellationToken);

        return Ok(result);
    }
}