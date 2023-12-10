using ApiClient.Discount.Models.Coupon;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices.Extensions;

public static class DiscountExtensions
{
    public static CouponDetail ToDetail(this CouponDetailModel model)
    {
        return new CouponDetail()
        {
            Amount = model.Amount,
            CatalogName = model.CatalogName,
            Description = model.Description,
            Id = model.Id,
            Type = (CatalogType)model.Type
        };
    }
}