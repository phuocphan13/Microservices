using AngularClient.Services;
using ApiClient.Catalog.SubCategory.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularClient.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SubCategoryController : ControllerBase
{
    private readonly ISubCategoryService _subCategoryService;

    public SubCategoryController(ISubCategoryService subCategoryService)
    {
            _subCategoryService = subCategoryService;
        }
    // Số 1
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
    public async Task<IActionResult> GetSubCategoryById (string id, CancellationToken cancellationToken)
    {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var result = await _subCategoryService.GetSubCategoryByIdAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }    
            return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetSubCategoryByName (string name, CancellationToken cancellationToken)
    {
            if(string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var result = await _subCategoryService.GetSubCategoryByNameAsync(name, cancellationToken);
            if(result == null)
            {  
                return NotFound();
            }    

            return Ok(result);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetSubCategoriesByCategoryId (string categoryId, CancellationToken cancellationToken)
    {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                return BadRequest();
            }

            var result = await _subCategoryService.GetSubCategoriesByCategoryIdAsync(categoryId, cancellationToken);

            if( result == null)
            {
                return NotFound();
            }    

            return Ok(result);
        }

    [HttpPost]
    public async Task<IActionResult> CreateSubCategory ([FromBody] CreateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
            var result = await _subCategoryService.CreateSubCategoryAsync(body, cancellationToken);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    [HttpPut]
    public async Task<IActionResult> UpdateSubCategory ([FromBody] UpdateSubCategoryRequestBody body, CancellationToken cancellationToken)
    {
            var result = await _subCategoryService.UpdateSubCategoryAsync(body, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubCategory(string id, CancellationToken cancellationToken)
    {
            var result = await _subCategoryService.DeleteSubCategoryAsync(id, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
    }
}