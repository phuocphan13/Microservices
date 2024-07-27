using Coupon.Grpc.Protos;

namespace AngularClient.Extensions
{
    public static class GrpcServiceExtensions
    {
        public static void AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpcClient<CouponProtoService.CouponProtoServiceClient>(options =>
            {
                options.Address = new Uri("http://localhost:5003");
            });
        }
    }
}
