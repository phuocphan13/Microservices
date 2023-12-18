using AngularClient.Services.Catalog;
using ApiClient.Catalog.Models.Catalog.Category;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers.Catalog;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetCategoryByName(string name, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoryByNameAsync(name, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateCategoryAsync(requestBody, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _categoryService.UpdateCategoryAsync(requestBody, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(string id, CancellationToken cancellationToken)
    {
        var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

        return Ok(result);
    }
}
