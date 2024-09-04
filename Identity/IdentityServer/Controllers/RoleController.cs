using ApiClient.DirectApiClients.Identity.RequestBodies;
using ApiClient.IdentityServer.Models.RequestBodies;
using Core.Common.Constants;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class RoleController : ApiController
{
    private readonly IRoleService _roleService;
    
    public RoleController(ILogger<RoleController> logger, IRoleService roleService) 
        : base(logger)
    {
        _roleService = roleService;
    }
    
    [HttpPost]
    // [Permission("")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequestBody requestBody)
    {
        if (string.IsNullOrWhiteSpace(requestBody.UserId))
        {
            return BadRequest("Invalid user id");
        }

        if (string.IsNullOrWhiteSpace(requestBody.UserId))
        {
            return BadRequest("Invalid role name");
        }

        var result = await _roleService.AssignRoleToUserAsync(requestBody.UserId, requestBody.RoleId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }
    
    [HttpPost]
    // [Permission("")]
    public async Task<IActionResult> RemoveRoleToUser([FromBody] AssignRoleToUserRequestBody requestBody)
    {
        if (string.IsNullOrWhiteSpace(requestBody.UserId))
        {
            return BadRequest("Invalid user id");
        }

        if (string.IsNullOrWhiteSpace(requestBody.UserId))
        {
            return BadRequest("Invalid role name");
        }

        var result = await _roleService.RemoveRoleToUserAsync(requestBody.UserId, requestBody.RoleId);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }
    
    [HttpPost]
    // [Permission("")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requestBody.RoleId))
        {
            return BadRequest("Invalid role id");
        }

        if (string.IsNullOrWhiteSpace(requestBody.FeatureId))
        {
            return BadRequest("Invalid feature id");
        }

        var result = await _roleService.AssignPermissionToRoleAsync(requestBody, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }

    [HttpPost]
    // [Permission("")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestBody requestBody)
    {
        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Invalid role name");
        }

        var isExisted = await _roleService.CheckRoleExistAsync(requestBody.Name, PropertyContstants.PropertyName.Name);

        if (isExisted)
        {
            return BadRequest("Role name is existed");
        }

        var result = await _roleService.CreateRoleAsync(requestBody);

        if (result is null)
        {
            return Problem("Role name is existed");
        }

        return Ok(result);
    }
    
    [HttpDelete]
    // [Permission("")]
    public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requestBody.RoleId))
        {
            return BadRequest("Invalid role id");
        }

        var result = await _roleService.RemoveRoleAsync(requestBody, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }
}