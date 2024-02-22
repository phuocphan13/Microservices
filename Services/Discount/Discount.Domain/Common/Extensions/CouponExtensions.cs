using ApiClient.Discount.Models.Coupon;
using Discount.Domain.Entities;

namespace Discount.Domain.Extensions;

public static class CouponExtensions
{
    public static CouponDetail ToDetail(this Coupon coupon)
    {
        return new CouponDetail()
        {
            Id = coupon.Id,
            Amount = coupon.Amount,
            Description = coupon.Description
        };
    }
    
    public static Coupon ToCreateCoupon(this CreateCouponRequestBody requestBody)
    {
        return new Coupon()
        {
            Name = requestBody.Name,
            Description = requestBody.Description,
            Amount = requestBody.Amount,
            FromDate = requestBody.FromDate,
            ToDate = requestBody.ToDate,
        };
    }

    public static Coupon ToUpdateCoupon(this UpdateCouponRequestBody requestBody)
    {
        return new Coupon()
        {
            Id = requestBody.Id!.Value,
            Name = requestBody.Name,
            Description = requestBody.Description,
            Amount = requestBody.Amount,
            FromDate = requestBody.FromDate,
            ToDate = requestBody.ToDate,
        };
    }
}