using Microsoft.AspNetCore.Mvc;
using System.Net;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Services;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsAsync(cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:length(24)}", Name = "GetProductById")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Missing Id.");
        }
        
        var result = await _productService.GetProductByIdAsync(id, cancellationToken);
        
        if (result is null)
        {
            _logger.LogError($"Product with id: {id}, not found.");
            return NotFound();
        }

        return Ok(result);
    }

    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    public async Task<IActionResult> GetProductByCategory(string category, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsByCategoryAsync(category, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }
        
        var result = await _productService.CreateProductAsync(requestBody, cancellationToken);
        
        if (!result.IsSuccessCode)
        {
            if (result.InternalErrorCode == 404)
            {
                return NotFound(result.Message);
            }
            
            if (result.InternalErrorCode == 500)
            {

                return Problem(result.Message); 
            }
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.Id))
        {
            return BadRequest("Product Id is not allowed null.");
        }

        var result = await _productService.UpdateProductAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem($"Cannot update product with name: {requestBody.Name}");
        }
            
        return Ok(result);
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    public async Task<IActionResult> DeleteProductById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Product Id is not allowed null.");
        }
        
        var result = await _productService.DeleteProductAsync(id, cancellationToken);
        
        return Ok(result);
    }
}
