using IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly ITokenHandleService _tokenHandleService;

    public IdentityController(ITokenHandleService tokenHandleService)
    {
        _tokenHandleService = tokenHandleService;
    }

    [HttpPost]
    public IActionResult GenerateToken(GenerateTokenRequest request)
    {
        var jwtToken = _tokenHandleService.GenerateTokenAsync(request);
        return Ok(jwtToken);
    }
}