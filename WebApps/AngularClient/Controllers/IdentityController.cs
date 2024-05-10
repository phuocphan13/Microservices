using AngularClient.Services;
using ApiClient.IdentityServer.Models;
using ApiClient.IdentityServer.Models.Request;
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

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Request is null");
        }

        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            return BadRequest("UserName is required");
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Password is required");
        }
        
        var result = await _identityService.LoginAsync(request, cancellationToken);

        if (result is null)
        {
            return Problem();
        }

        return Ok(result);
    }
}