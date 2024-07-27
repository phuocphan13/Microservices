using AngularClient.Extensions;
using ApiClient.Discount.Models.Coupon;
using Coupon.Grpc.Protos;

namespace AngularClient.Services
{
    public interface ICouponService
    {
        Task<List<ApiClient.Discount.Models.Coupon.CouponSummary>?> GetAllCouponsAsync(CancellationToken cancellationToken);

    }

    public class CouponService :ICouponService
    {
        private readonly CouponProtoService.CouponProtoServiceClient _couponProtoService;

        public CouponService(CouponProtoService.CouponProtoServiceClient couponProtoServiceClient)
        {
            _couponProtoService = couponProtoServiceClient;
        }

        public async Task<List<ApiClient.Discount.Models.Coupon.CouponSummary>?> GetAllCouponsAsync(CancellationToken cancellationToken)
        {
            var request = new Empty();

            var response = await _couponProtoService.GetAllCouponsAsync(request, cancellationToken: cancellationToken);

            return response.ToSummaries();
        }
    }
}
