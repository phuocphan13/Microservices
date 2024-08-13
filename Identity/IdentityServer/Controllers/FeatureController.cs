using ApiClient.IdentityServer.Models.RequestBodies;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class FeatureController : ApiController
{
    private readonly IFeatureService _featureService;
    
    public FeatureController(ILogger<FeatureController> logger, IFeatureService featureService) 
        : base(logger)
    {
        _featureService = featureService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFeature([FromBody] CreateFeatureRequestBody requestBody)
    {
        if (string.IsNullOrWhiteSpace(requestBody.ApplicationId))
        {
            return BadRequest("Invalid application id");
        }

        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Invalid feature name");
        }

        var result = await _featureService.CreateFeatureAsync(requestBody);

        if (result is null)
        {
            return Problem("Feature is created unsuccessfully");
        }

        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveFeature([FromQuery] string featureId)
    {
        if (string.IsNullOrWhiteSpace(featureId))
        {
            return BadRequest("Invalid feature id");
        }

        if (!Guid.TryParse(featureId, out Guid guiId))
        {
            return BadRequest("Invalid feature id");
        }
        
        var result = await _featureService.RemoveFeatureAsync(guiId);

        if (!result)
        {
            return Problem("Feature is removed unsuccessfully");
        }

        return Ok();
    }
}