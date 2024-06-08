using ApiClient.Discount.Enum;
using ApiClient.Discount.Models.Coupon;

namespace Discount.Domain.Extensions;

public static class CouponExtensions
{
    public static CouponDetail ToDetail(this Entities.Coupon coupon)
    {
        return new CouponDetail()
        {
            Id = coupon.Id,
            Value = coupon.Value,
            Type = (int)coupon.Type,
            Description = coupon.Description
        };
    }
    
    public static Entities.Coupon ToCreateCoupon(this CreateCouponRequestBody requestBody)
    {
        return new Entities.Coupon()
        {
            Name = requestBody.Name,
            Description = requestBody.Description,
            Value = requestBody.Value,
            Type = (CouponEnum)requestBody.Type,
            FromDate = requestBody.FromDate,
            ToDate = requestBody.ToDate,
        };
    }

    public static void ToUpdateCoupon(this Entities.Coupon entity, UpdateCouponRequestBody requestBody)
    {
        entity.Name = requestBody.Name;
        entity.Description = requestBody.Description;
        entity.Value = requestBody.Value;
        entity.Type = (CouponEnum)requestBody.Type;
    }
}