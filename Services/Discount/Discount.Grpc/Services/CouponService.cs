using ApiClient.Discount.Models.Coupon;
using AutoMapper;
using Coupon.Grpc.Protos;
using Discount.Domain.Services;
using Grpc.Core;

namespace Discount.Grpc.Services;

// ------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE -----------------
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
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Name cannot be null"));
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Description cannot be null"));
        }

        if (string.IsNullOrEmpty(request.Value))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Value cannot be null"));
        }

        if (string.IsNullOrEmpty(request.Type.ToString()))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Type cannot be null"));
        }

        var existingCoupon = await _couponService.GetCouponForCreateAsync(request.Name);
        if (existingCoupon != null)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, $"Coupon already exists."));
        }    

        var requestBody = _mapper.Map<CreateCouponRequestBody>(request);
        var result = await _couponService.CreateCouponAsync(requestBody);

        if (result is null)
        {
            _logger.LogError("Coupon creation failed.");
            throw new RpcException(new Status(StatusCode.Internal, "Coupon creation failed"));
        }

        var couponModel = _mapper.Map<CouponDetailModel>(requestBody);

        return couponModel;
    }

    // ------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE -----------------
    public override async Task<CouponDetailModel> UpdateCoupon(EditCouponRequest request, ServerCallContext context)
    {
        if (request is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Request cannot be null."));
        }

        if (request.Id == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Id cannot be null."));
        }

        var coupon = await _couponService.GetCouponAsync(request.Id.ToString());

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Product Id = {request.Id} is not existed"));
        }

        //Done-Trung: Add Validation for other fields and Check if Entity is Exist

        var requestBody = _mapper.Map<UpdateCouponRequestBody>(request);

        var result = await _couponService.UpdateCouponAsync(requestBody);

        if (result is null)
        {
            _logger.LogError("Coupon update failed.");
        }

        _logger.LogInformation($"Coupon updated successfully.");

        var couponModel = _mapper.Map<CouponDetailModel>(result);

        return couponModel;
    }
    // ------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE ----------------- GRPC SERVICE -----------------

    public override async Task<InactiveCouponResponse> InactiveCoupon(InactiveCounponRequest request, ServerCallContext context)
    {
        //done-Trung: Add Validation for other fields and Check if Entity is Exist
        if (request is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Request cannot be null."));
        }

        if (request.Id == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Request cannot be null."));
        }
        var coupon = await _couponService.GetCouponAsync(request.Id.ToString());

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Product Id = {request.Id} is not existed"));
        }

        var result = await _couponService.InactiveCoupon(request.Id);

        if (result is false)
        {
            _logger.LogError("Coupon delete failed.");
        }

        _logger.LogInformation("Coupon delete is successfully.");

        var response = new InactiveCouponResponse()
        {
            Success = true
        };

        return response;
    }
}