﻿using System.Text;
using Microsoft.AspNetCore.Mvc;
using ApiClient.Catalog.Product.Models;
using ApiClient.Common.Models.Paging;
using Catalog.API.Services;
using Platform.ApiBuilder;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ProductController : ApiController
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISubCategoryService _subCategoryService;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(IProductService productService, ICategoryService categoryService, 
        ISubCategoryService subCategoryService, ILogger<ProductController> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(productService);
        ArgumentNullException.ThrowIfNull(categoryService);
        ArgumentNullException.ThrowIfNull(subCategoryService);
        ArgumentNullException.ThrowIfNull(logger);

        _productService = productService;
        _categoryService = categoryService;
        _subCategoryService = subCategoryService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPagingProducts([FromQuery] PagingInfo pagingInfo, CancellationToken cancellationToken)
    {
        if (pagingInfo is null)
        {
            return BadRequest("Missing PagingInfo.");
        }

        var result = await _productService.GetPagingProductsAsync(pagingInfo, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        _logger.LogInformation("Get all products paging successfully.");

        return base.Ok(result);
    }

    [HttpGet]
    //[Permission(PermissionConstants.Feature.CatalogApi.GetAllProducts)]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsAsync(cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        _logger.LogInformation("Get all products successfully.");
         
        return Ok(result);
    }

    [HttpGet("{id}")]
    // [Permission(PermissionConstants.Feature.CatalogApi.GetProductById)]
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

    [Route("[action]/{category}")]
    [HttpGet]
    public async Task<IActionResult> GetProductByCategory(string category, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return BadRequest("Missing Category.");
        }

        var result = await _productService.GetProductsByCategoryAsync(category, cancellationToken);

        if (result is null || result.Count == 0)
        {
            _logger.LogError($"Product with category: {category}, not found.");
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsByListCodes([FromQuery] List<string> codes, CancellationToken cancellationToken)
    {
        if (codes is null || codes.Count == 0)
        {
            return BadRequest("Missing Codes.");
        }

        var result = await _productService.GetProductsByListCodesAsync(codes, cancellationToken);

        if (result is null || result.Count == 0)
        {
            _logger.LogError($"Product with codes: {string.Join(",", codes)}, not found.");
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
        
        var category = await _categoryService.GetCategoryBySeachAsync(requestBody.CategoryId, PropertyName.Id, cancellationToken);

        if (category is null)
        {
            return "Category is not existed.";
        }

        if (string.IsNullOrWhiteSpace(requestBody.SubCategoryId))
        {
            return "Sub-Category Id is not allowed null.";
        }

        var isSubCategoryExist = await _subCategoryService.CheckExistingAsync(requestBody.SubCategoryId, PropertyName.Id, cancellationToken);

        if (!isSubCategoryExist)
        {
            return "Sub-Category is not existed.";
        }

        return string.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var jsonContent =
            """
            
                    {
                        "query": {
                            "match_all": {}
                        }
                    }
            """;

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // string url = "http://192.168.2.11:9200/logs-otel/_search?pretty";
        string url = "http://127.0.0.1:9200/_cat/indices?v";

        var response = await client.GetAsync(url, cancellationToken);

        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

        return Ok(responseString);
    }
}

public static partial class Log
{
    [LoggerMessage(LogLevel.Information, "Get Product Success Luficer")]
    public static partial void GetProducts(this ILogger logger);
}