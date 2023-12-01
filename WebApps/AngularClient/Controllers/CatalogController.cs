using AngularClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _catalogService.GetProductsAsync(cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductById(string id, CancellationToken cancellationToken)
    {
        var result = await _catalogService.GetProductById(id, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{category}")]
    public async Task<IActionResult> GetProductByCategory(string category, CancellationToken cancellationToken)
    {
        var result = await _catalogService.GetProductByCategoryAsync(category, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}