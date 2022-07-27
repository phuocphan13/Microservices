using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);

        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name = {request.ProductName} is not existed"));
        }

        _logger.LogInformation($"Discount is retrieved for Product Name: {request.ProductName} with Amount {coupon.Amount}");

        var couponModel = _mapper.Map<CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        var result = await _repository.CreateDiscount(coupon);
        if (!result)
        {
            _logger.LogError("Discount is failed created.");
        }

        _logger.LogInformation($"Discount is successfully created. Product Name: {coupon.ProductName}");
        var couponModel = _mapper.Map<CouponModel>(coupon); 

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request.Coupon);
        var result = await _repository.UpdateDiscount(coupon);
        if (!result)
        {
            _logger.LogError("Discount is failed updated.");
        }

        _logger.LogInformation($"Discount is successfully updated. Product Name: {coupon.ProductName}");
        var couponModel = _mapper.Map<CouponModel>(coupon);

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _repository.DeleteDiscount(request.ProductName);
        if (!result)
        {
            _logger.LogError("Discount is failed deleted.");
        }

        _logger.LogInformation($"Discount is successfully deleted. Product Name: {request.ProductName}");

        var response = new DeleteDiscountResponse()
        {
            Success = result
        };

        return response;
    }
}
