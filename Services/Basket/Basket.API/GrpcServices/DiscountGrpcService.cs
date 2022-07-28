using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices;

public interface IDiscountGrpcService
{
    Task<CouponModel> GetDiscount(string productName);
}

public class DiscountGrpcService : IDiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcService)
    {
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest()
        { 
            ProductName = productName }
        ;

        var couponModel = await _discountGrpcService.GetDiscountAsync(discountRequest);

        return couponModel;
    }
}
