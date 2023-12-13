using ApiClient.Discount.Models.Discount;
using Discount.Domain.Entities;

namespace Discount.Domain.Extensions;

public static class DiscountVersionExtensions
{
    public static DiscountDetail ToDetail(this DiscountVersion entity)
    {
        return new DiscountDetail()
        {
            Id = entity.Id
        };
    }
    
    public static DiscountVersion ToCreateDiscountVersion(this CreateDiscountRequestBody body, string couponId)
    {
        return new DiscountVersion()
        {
            CouponId = couponId,
            FromDate = body.FromDate,
            ToDate = body.ToDate
        };
    }
}