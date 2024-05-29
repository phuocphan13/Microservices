using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Domain.Services;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class CouponService : CouponProtoService.CouponProtoServiceBase
{
    private readonly ILogger<CouponService> _logger;
    private readonly IMapper _mapper;
    private readonly ICouponService _couponService;

    public CouponService(ILogger<CouponService> logger, IMapper mapper, ICouponService couponService)
    {
        _logger = logger;
        _mapper = mapper;
        _couponService = couponService;
    }

    public override async Task<CouponDetailModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Id cannot be null."));
        }
        
        var coupon = await _couponService.GetCouponAsync(request.Id);
        
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Product Id = {request.Id} is not existed"));
        }
        
        _logger.LogInformation($"Coupon is retrieved for Id: {request.Id}");
        
        var couponModel = _mapper.Map<CouponDetailModel>(coupon);
        return couponModel;
    }
}