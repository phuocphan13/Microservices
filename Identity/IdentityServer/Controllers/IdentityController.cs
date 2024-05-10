using ApiClient.IdentityServer.Models;
using ApiClient.IdentityServer.Models.Request;
using IdentityServer.Domain.Entities;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[Route("api/[controller]/[action]")]
public class IdentityController : ApiController
{
    private readonly ITokenHandleService _tokenHandleService;
    private readonly UserManager<Account> _userManager;

    public IdentityController(ITokenHandleService tokenHandleService, UserManager<Account> userManager, ILogger<IdentityController> logger) : base(logger)
    {
        _tokenHandleService = tokenHandleService;
        _userManager = userManager;
    }

    // [HttpPost]
    // public IActionResult GenerateToken(GenerateTokenRequest request)
    // {
    //     var jwtToken = _tokenHandleService.GenerateTokenAsync(request);
    //     return Ok(jwtToken);
    // }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            return BadRequest("Invalid username or password");
        }

        if (!user.IsActive)
        {
            return BadRequest("Username đã bị vô hiệu.");
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return BadRequest("Invalid username or password");
        }
        
        var token = await _tokenHandleService.LoginAsync(user, cancellationToken);

        if (token is null)
        {
            return BadRequest("Failed to generate token.");
        }
        
        return Ok(token);
    }
}