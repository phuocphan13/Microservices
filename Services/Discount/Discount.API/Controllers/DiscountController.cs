using Microsoft.AspNetCore.Mvc;
using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Entities;
using Discount.Domain.Services;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    private readonly ILogger<DiscountController> _logger;

    public DiscountController(ILogger<DiscountController> logger, IDiscountService discountService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
    }

    [HttpGet]
    public async Task<ActionResult<Coupon>> GetDiscount([FromQuery] string searchText, [FromQuery] CatalogType type)
    {
        var coupon = await _discountService.GetDiscountByTextAsync(searchText, type);
        return Ok(coupon);
    }

    [HttpPost]
    public async Task<ActionResult<CouponDetail>> CreateDiscount([FromBody] CreateCouponRequestBody requestBody)
    {
        var coupon = await _discountService.CreateDiscountAsync(requestBody);
        return Ok(coupon);
    }

    [HttpPut]
    public async Task<ActionResult<CouponDetail>> UpdateDiscount([FromBody] UpdateCouponRequestBody requestBody)
    {
        var coupon = await _discountService.UpdateDiscountAsync(requestBody);
        return Ok(coupon);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteDiscount(int id)
    {
        var result = await _discountService.DeleteDiscountAsync(id);
        return Ok(result);
    }
}
