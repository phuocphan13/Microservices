using AngularClient.Services.Catalog.Product;
using ApiClient.Catalog.Models.Catalog.Product;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers.Catalog.Product;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public ProductController(ICatalogService catalogService)
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

    [HttpGet("{id}")]
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _catalogService.CreateProductAsync(requestBody, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _catalogService.UpdateProductAsync(requestBody, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
    {
        var result = await _catalogService.DeleteProductAsync(id, cancellationToken);

        return Ok(result);
    }
}