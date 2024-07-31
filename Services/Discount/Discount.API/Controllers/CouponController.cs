using Microsoft.AspNetCore.Mvc;
using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Services;
using Platform.ApiBuilder;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CouponController : ApiController
{
    private readonly ICouponService _couponService;
    private readonly ILogger<CouponController> _logger;

    public CouponController( ICouponService couponService, ILogger<CouponController> logger) : base(logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
    }

    [HttpGet]
    public async Task<ActionResult<List<CouponSummary>?>> GetAllCoupons()
    {
        var listCoupon = await _couponService.GetAllCouponsAsync();

        if(listCoupon is null)
        { return NotFound(); }

        return Ok(listCoupon);
    }

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
}
