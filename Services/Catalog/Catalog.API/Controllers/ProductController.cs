using Microsoft.AspNetCore.Mvc;
using ApiClient.Catalog.Product.Models;
using Catalog.API.Services;
using Platform.ApiBuilder;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ApiController
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISubCategoryService _subCategoryService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ICategoryService categoryService, 
        ISubCategoryService subCategoryService, ILogger<ProductController> logger) : base(logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _subCategoryService = subCategoryService ?? throw new ArgumentNullException(nameof(subCategoryService));
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
        if (string.IsNullOrWhiteSpace(category))
        {
            return BadRequest("Missing Category.");
        }

        var result = await _productService.GetProductsByCategoryAsync(category, cancellationToken);

        if (result is null || !result.Any())
        {
            _logger.LogError($"Product with category: {category}, not found.");
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Name is not allowed null.");
        }

        var isExsited = await _productService.CheckExistingAsync(requestBody.Name, PropertyName.Name, cancellationToken);

        if (isExsited)
        {
            return BadRequest("Product name is existed.");
        }

        var validationMsg = await ValidationRequestBody(requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(validationMsg))
        {
            return BadRequest(validationMsg);
        }

        var result = await _productService.CreateProductAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Create product failed.");
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

        var isExsited = await _productService.CheckExistingAsync(requestBody.Id, PropertyName.Id, cancellationToken);

        if (!isExsited)
        {
            return BadRequest("Product is not existed.");
        }

        var validationMsg = await ValidationRequestBody(requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(validationMsg))
        {
            return BadRequest(validationMsg);
        }

        var result = await _productService.UpdateProductAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Update product failed.");
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

    private async Task<string> ValidationRequestBody<T>(T requestBody, CancellationToken cancellationToken)
        where T : BaseProductRequestBody
    {
        if (string.IsNullOrWhiteSpace(requestBody.CategoryId))
        {
            return "Category Id is not allowed null.";
        }
        
        var catalog = await _categoryService.GetCategoryBySeachAsync(requestBody.CategoryId, PropertyName.Id, cancellationToken);

        if (catalog is null)
        {
            return "Category is not existed.";
        }

        if (string.IsNullOrWhiteSpace(requestBody.SubCategoryId))
        {
            return "Sub-Category Id is not allowed null.";
        }

        var isSubCatalogExist = await _subCategoryService.CheckExistingAsync(requestBody.SubCategoryId, PropertyName.Id, cancellationToken);

        if (!isSubCatalogExist)
        {
            return "Sub-Category is not existed.";
        }

        return string.Empty;
    }
}