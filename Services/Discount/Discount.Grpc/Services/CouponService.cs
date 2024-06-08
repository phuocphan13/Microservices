using ApiClient.Discount.Models.Coupon;
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

    public override async Task<CouponDetailModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
    {
        //Todo-Phat: Add Validation for other fields and Check if Entity is Exist
        
        var requestBody = _mapper.Map<CreateCouponRequestBody>(request);
        var result = await _couponService.CreateCouponAsync(requestBody);

        if (result is null)
        {
            _logger.LogError("Discount is failed created.");
        }

        var couponModel = _mapper.Map<CouponDetailModel>(requestBody);

        return couponModel;
    }
}