using Microsoft.AspNetCore.Mvc;
using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Services;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;
    private readonly ILogger<CouponController> _logger;

    public CouponController(ILogger<CouponController> logger, ICouponService couponService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
    }

    // [HttpGet]
    // public async Task<ActionResult<Domain.Entities.Coupon>> GetDiscount([FromQuery] string searchText, [FromQuery] CatalogType type)
    // {
    //     var coupon = await _couponService.GetCouponByTextAsync(searchText, type);
    //     return Ok(coupon);
    // }

    [HttpPost]
    public async Task<ActionResult<CouponDetail>> CreateDiscount([FromBody] CreateCouponRequestBody requestBody)
    {
        var coupon = await _couponService.CreateCouponAsync(requestBody);
        return Ok(coupon);
    }

    [HttpPut]
    public async Task<ActionResult<CouponDetail>> UpdateDiscount([FromBody] UpdateCouponRequestBody requestBody)
    {
        var coupon = await _couponService.UpdateCouponAsync(requestBody);
        return Ok(coupon);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteDiscount(int id)
    {
        var result = await _couponService.DeleteCouponAsync(id);
        return Ok(result);
    }
}
