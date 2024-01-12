using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using static Catalog.API.Common.Consts.ResponseMessages;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class SubCategoryController : ControllerBase
{
    private readonly ISubCategoryService _subCategoryService;

    public SubCategoryController(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService ?? throw new ArgumentNullException(nameof(subCategoryService));
    }

    [HttpGet]
    public async Task<IActionResult> GetSubCategories(CancellationToken cancellationToken)
    {
        var result = await _subCategoryService.GetSubCategoriesAsync(cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubCategoryById(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id cannot be empty.");
        }

        var result = await _subCategoryService.GetSubCategoryByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetSubCategoryByName(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Missing Name.");
        }

        var result = await _subCategoryService.GetSubCategoryByNameAsync(name, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("requestBody is null.");
        }
        
        if (string.IsNullOrWhiteSpace(requestBody.Name))
        {
            return BadRequest("Name is null.");
        }
        
        if (string.IsNullOrWhiteSpace(requestBody.SubCategoryCode))
        {
            return BadRequest("SubCategoryCode is null.");
        }
        
        if (string.IsNullOrWhiteSpace(requestBody.CategoryId))
        {
            return BadRequest("CategoryId is null.");
        }

        var result = await _subCategoryService.CreateSubCategoryAsync(requestBody, cancellationToken);

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
    public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("requestBody is null.");
        }
        
        if (string.IsNullOrWhiteSpace(requestBody.Id))
        {
            return BadRequest("Id is null.");
        }

        var result = await _subCategoryService.UpdateSubCategoryAsync(requestBody, cancellationToken);

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
    public async Task<IActionResult> DeleteSubCategory(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Product Id is not allowed null.");
        }

        var result = await _subCategoryService.DeleteSubCategoryAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetSubCategoriesByCategoryId(string categoryId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            return BadRequest("Missing CategoryId");
        }

        var result = await _subCategoryService.GetSubCategoriesByCategoryIdAsync(categoryId, cancellationToken);
        
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}