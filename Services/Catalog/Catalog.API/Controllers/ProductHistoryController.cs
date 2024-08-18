using ApiClient.Catalog.ProductHistory.Models;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ProductHistoryController : ApiController
{
    private readonly IProductService _productService;
    private readonly IProductHistoryService _productHistoryService;
    
    public ProductHistoryController(ILogger<ProductHistoryController> logger, IProductService productService, IProductHistoryService productHistoryService) 
        : base(logger)
    {
        _productService = productService;
        _productHistoryService = productHistoryService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductBalanceAsync(AddProductBalanceRequestBody requestBody, CancellationToken cancellationToken)
    {
        var isExisted = await _productService.CheckExistingAsync(requestBody.Id, PropertyName.Id, cancellationToken);

        if (!isExisted)
        {
            return BadRequest("Product not found.");
        }
        
        var result = await _productHistoryService.AddProductBalanceAsync(requestBody, cancellationToken);

        if (!result)
        {
            return Problem("Failed to add product balance.");
        }

        return Ok();
    }
}