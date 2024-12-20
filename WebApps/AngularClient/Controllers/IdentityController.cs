using AngularClient.Services;
using ApiClient.IdentityServer.Models;
using ApiClient.IdentityServer.Models.RequestBodies;
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
    public async Task<IActionResult> Login([FromBody] LoginRequestBody request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Request is null");
        }

        if (string.IsNullOrWhiteSpace(request.UserName))
        {
            return BadRequest("UserName is required");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
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

    [HttpPost]
    public async Task<IActionResult> GenerateAccessTokenByRefreshToken([FromBody] GenerateAccessTokenByRefreshTokenRequestBody request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Invalid request");
        }

        if (string.IsNullOrWhiteSpace(request.AccountId))
        {
            return BadRequest("Invalid account id");
        }

        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest("Invalid refresh token");
        }

        var result = await _identityService.GenerateAccessTokenByRefreshTokenAsync(request, cancellationToken);

        return Ok(result);
    }
}