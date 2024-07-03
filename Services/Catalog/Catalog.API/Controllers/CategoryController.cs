using ApiClient.Catalog.Models.Catalog.Category;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CategoryController : ApiController
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : base(logger)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoriesAsync(cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetCategoryByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The Name field cannot be null");
        }

        var result = await _categoryService.GetCategoryBySeachAsync(name, PropertyName.Name, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("The Id filed cannot be null");
        }

        var result = await _categoryService.GetCategoryBySeachAsync(id, PropertyName.Id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Category Name is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.CategoryCode))
        {
            return BadRequest("Category Code is not allowed null.");
        }

        var result = await _categoryService.CreateCategoryAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Cannot create category.");
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Category Name is not allowed null.");
        }

        if (string.IsNullOrWhiteSpace(requestBody.CategoryCode))
        {
            return BadRequest("Category Code is not allowed null.");
        }

        var result = await _categoryService.UpdateCategoryAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Cannot update category.");
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Category Id is not allowed null.");
        }

        var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

        return Ok(result);
    }
}