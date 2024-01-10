using ApiClient.Catalog.Models.Catalog.Category;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
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
        if(string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The Name field cannot be null");
        }

        var result = await _categoryService.GetCategoryByNameAsync(name, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(string id, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("The Id filed cannot be null");
        }    

        var result = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

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

        if (!result.IsSuccessCode)
        {
            if (result.InternalErrorCode == 500)
            {
                return Problem(result.Message);
            }
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
        if (!result.IsSuccessCode)
        {
            if (result.InternalErrorCode == 404)
            {
                return NotFound(result);
            }

            if (result.InternalErrorCode == 500)
            {

                return Problem(result.Message);
            }
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

        if (result is null)
        {
            return Problem($"Cannot delete category with id: {id}");
        }

        return Ok(result);
    }
}