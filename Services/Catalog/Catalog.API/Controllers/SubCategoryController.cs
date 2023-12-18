using ApiClient.Catalog.Models.SubCategory;
using ApiClient.Common;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using static Catalog.API.Common.Consts.ResponseMessages;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class SubCatagoryController : ControllerBase
{
    private readonly ISubCategoryService _subCategory;
    private object apiDataResult;

    public SubCatagoryController(ISubCategoryService subCategory)
    {
        _subCategory = subCategory;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubCategory(CancellationToken cancellationToken)
    {
        var result = await _subCategory.GetSubCategoriesAsync(cancellationToken);
        if(result == null)
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
            return BadRequest("Missing Id.");
        }

        var result = await _subCategory.GetSubCategoryByIdAsync(id, cancellationToken);

        if (result is null)
        { 
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetSubCategoryByName (string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Missing Name.");
        }

        var result = await _subCategory.GetSubCategoryByNameAsync(name, cancellationToken);
        if( result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryRequestBody requestBody, CancellationToken cancellationToken)
    {
        var result = await _subCategory.CreateSubCategoryAsync(requestBody, cancellationToken);

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
        var result = await _subCategory.UpdateSubCategoryAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem($"Cannot update product with name: {requestBody.Name}");
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

        var result = await _subCategory.DeleteSubCategoryAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetSubCategoryByCategoryId (string categoryId, CancellationToken cancellationToken)
    {
        var result = await  _subCategory.GetSubCategoryByCategoryIdAsync(categoryId, cancellationToken);

        return Ok(result);
    }
}