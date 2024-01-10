using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ValidationController : ControllerBase
{
    private readonly IValidationService _validationService;

    public ValidationController(IValidationService validationService)
    {
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<IActionResult> ValidateCatalogCode([FromQuery] string? catalogCode, [FromQuery] DiscountEnum? type, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(catalogCode))
        {
            return BadRequest("CatalogCode cannot be blank.");
        }

        if (type is null)
        {
            return BadRequest("Type cannot be blank.");
        }

        var isValid = await _validationService.ValidateCatalogCodeAsync(catalogCode, type.Value, cancellationToken);

        return Ok(isValid);
    }
}