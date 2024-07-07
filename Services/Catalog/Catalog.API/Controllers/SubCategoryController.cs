using ApiClient.Catalog.SubCategory.Models;
using ApiClient.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;
using static Catalog.API.Common.Consts.ResponseMessages;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class SubCategoryController : ApiController
{
    private readonly ISubCategoryService _subCategoryService;
    private readonly ICategoryService _categoryService;

    public SubCategoryController(ISubCategoryService subCategoryService, ICategoryService categoryService, ILogger<SubCategoryController> logger) : base(logger)
    {
        _subCategoryService = subCategoryService ?? throw new ArgumentNullException(nameof(subCategoryService));
        _categoryService = categoryService;
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

        var result = await _subCategoryService.GetSubCategoryBySeachAsync(id, PropertyName.Id, cancellationToken);

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

        var result = await _subCategoryService.GetSubCategoryBySeachAsync(name, PropertyName.Name, cancellationToken);
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

        var validationMsg = await ValidationRequestBody(requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(validationMsg))
        {
            return BadRequest(validationMsg);
        }

        var result = await _subCategoryService.CreateSubCategoryAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Create Subcategory failed");
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

        var validationMsg = await ValidationRequestBody(requestBody, cancellationToken);

        if (!string.IsNullOrWhiteSpace(validationMsg))
        {
            return BadRequest(validationMsg);
        }

        var subCategory = await _subCategoryService.GetSubCategoriesAsync(cancellationToken);
        //var isExisted = await _subCategoryRepository.AnyAsync(x => (x.Name == body.Name || x.SubCategoryCode == body.SubCategoryCode) && x.Id != body.Id, cancellationToken);
        foreach (var item in subCategory)
        {
            if ((item.Name == requestBody.Name || item.SubCategoryCode == requestBody.SubCategoryCode) && item.Id != requestBody.Id)
            {
                return BadRequest("Updated information already exists."); ;
            }
        }    

        var result = await _subCategoryService.UpdateSubCategoryAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return Problem("Update Subcategory failed");
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

    private async Task<string> ValidationRequestBody<T>(T requestBody, CancellationToken cancellationToken)
        where T : BaseSubCategoryResquestBody
    {
        if (string.IsNullOrWhiteSpace(requestBody.CategoryId))
        {
            return "Category Id is not allowed null.";
        }

        var isCatalogExist = await _categoryService.CheckExistingAsync(requestBody.CategoryId, PropertyName.Id, cancellationToken);

        if (!isCatalogExist)
        {
            return "Category is not existed.";
        }

        return string.Empty;
    }
}