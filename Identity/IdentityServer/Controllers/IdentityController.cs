using ApiClient.IdentityServer.Models;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[Route("api/[controller]/[action]")]
public class IdentityController : ApiController
{
    private readonly ITokenHandleService _tokenHandleService;

    public IdentityController(ITokenHandleService tokenHandleService, ILogger<IdentityController> logger) : base(logger)
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