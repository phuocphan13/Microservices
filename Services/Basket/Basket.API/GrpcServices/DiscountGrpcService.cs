using ApiClient.Discount.Models.Coupon;
using Basket.API.GrpcServices.Extensions;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public interface IDiscountGrpcService
{
    Task<CouponDetail> GetDiscount(string productName);
}

public class DiscountGrpcService : IDiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcService)
    {
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }

    public async Task<CouponDetail> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest()
        { 
            Id = productName
        };

        DiscountDetailModel couponModel;

        try
        {
            couponModel = await _discountGrpcService.GetDiscountAsync(discountRequest);
        }
        catch (Exception)
        {
            couponModel = new DiscountDetailModel();
        }

        return couponModel.ToDetail();
    }
}
