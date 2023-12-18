using ApiClient.Discount.Models.Coupon;
using AutoMapper;
using Discount.Domain.Services;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly ICouponService _couponService;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(ICouponService couponService, ILogger<DiscountService> logger, IMapper mapper)
    {
        _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<CouponDetailModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _couponService.GetDiscountByTextAsync(request.SearchText, (CatalogType)request.Type);
    
        if (coupon is null)
        {
            // throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name = {request.ProductName} is not existed"));
        }
    
        // _logger.LogInformation($"Discount is retrieved for Product Name: {request.ProductName} with Amount {coupon.Amount}");
    
        var couponModel = _mapper.Map<CouponDetailModel>(coupon);
        return couponModel;
    }
    
    public override async Task<CouponDetailModel> CreateDiscount(CreateCouponRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<CreateCouponRequestBody>(request);
        var result = await _couponService.CreateDiscountAsync(requestBody);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed created.");
        }
    
        _logger.LogInformation($"Discount is successfully created. Product Name: {requestBody.Code}");
        var couponModel = _mapper.Map<CouponDetailModel>(requestBody); 
    
        return couponModel;
    }
    
    public override async Task<CouponDetailModel> UpdateDiscount(UpdateCouponRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<UpdateCouponRequestBody>(request);
        
        var result = await _couponService.UpdateDiscountAsync(requestBody);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed updated.");
        }
    
        _logger.LogInformation($"Discount is successfully updated. Product Name: {requestBody.Code}");
        var couponModel = _mapper.Map<CouponDetailModel>(requestBody);
    
        return couponModel;
    }
    
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _couponService.DeleteDiscountAsync(request.Id);
        if (!result)
        {
            _logger.LogError("Discount is failed deleted.");
        }
    
        _logger.LogInformation($"Discount is successfully deleted. Product Name: {request.Id}");
    
        var response = new DeleteDiscountResponse()
        {
            Success = result
        };
    
        return response;
    }
}
