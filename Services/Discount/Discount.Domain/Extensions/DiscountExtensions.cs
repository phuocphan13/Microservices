using ApiClient.Discount.Models.Discount;
using Discount.Domain.Entities;

namespace Discount.Domain.Extensions;

public static class DiscountExtensions
{
    public static DiscountDetail ToDetail(this Entities.Discount entity)
    {
        return new DiscountDetail()
        {
            Id = entity.Id
        };
    }
    
    public static Entities.Discount ToCreateDiscountVersion(this CreateDiscountRequestBody body, string couponId)
    {
        return new Entities.Discount()
        {
            CouponId = couponId,
            FromDate = body.FromDate,
            ToDate = body.ToDate
        };
    }
}