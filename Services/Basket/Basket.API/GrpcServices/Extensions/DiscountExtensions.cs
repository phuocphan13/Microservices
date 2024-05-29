using ApiClient.Discount.Models.Coupon;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices.Extensions;

public static class DiscountExtensions
{
    public static CouponDetail ToDetail(this DiscountDetailModel model)
    {
        return new CouponDetail()
        {
            // Amount = model.Amount,
            Description = model.Description,
            Id = model.Id,
        };
    }
}