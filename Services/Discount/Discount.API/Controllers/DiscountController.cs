using ApiClient.Discount.Models.Discount;
using Discount.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetListDiscountsByCatalogCodeAsync([FromQuery] int type, [FromQuery] List<string> catalogCodes)
    {
        if (catalogCodes is null || catalogCodes.Count == 0)
        {
            return BadRequest("CatalogCodes is not allowed null or empty.");
        }

        var result = await _discountService.GetListDiscountsByCatalogCodeAsync((DiscountEnum)type, catalogCodes);

        if (result is null || result.Count == 0)
        {
            return Ok(new List<DiscountDetail>());
        }
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetDiscountByCatalogCode([FromQuery] int type, [FromQuery] string catalogCode)
    {
        if (string.IsNullOrWhiteSpace(catalogCode))
        {
            return BadRequest("CatalogCode is not allowed null or empty.");
        }

        var result = await _discountService.GetDiscountByCatalogCodeAsync(type, catalogCode);

        if (result is null)
        {
            return NotFound($"Cannot find Discount with CatalogCode: {catalogCode}");
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountRequestBody requestBody, CancellationToken cancellationToken)
    {
        if (requestBody is null)
        {
            return BadRequest("RequestBody is not allowed null.");
        }

        var result = await _discountService.CreateDiscountAsync(requestBody, cancellationToken);

        if (result is null)
        {
            return BadRequest($"Cannot create Discount with CatalogCode: {requestBody.CatalogCode}");
        }
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> InactiveDiscount(int? id)
    {
        if (id is null || id == 0)
        {
            return BadRequest("Id is not allowed null or O.");
        }

        var result = await _discountService.InactiveDiscountAsync(id.Value);

        return Ok(result);
    }
}