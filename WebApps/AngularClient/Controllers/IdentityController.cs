using AngularClient.Services;
using ApiClient.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.GenerateTokenAsync(request, cancellationToken);

        if (result is null)
        {
            return Problem();
        }

        return Ok(result);
    }
}