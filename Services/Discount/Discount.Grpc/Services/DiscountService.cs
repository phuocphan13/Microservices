using ApiClient.Discount.Models.Discount;
using AutoMapper;
using Discount.Domain.Services;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountService _discountService;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountService discountService, ILogger<DiscountService> logger, IMapper mapper)
    {
        _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task<DiscountDetailModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Id cannot be null."));
        }
        
        var discount = await _discountService.GetDiscountAsync(request.Id);
    
        if (discount is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Id = {request.Id} is not existed"));
        }
    
        _logger.LogInformation($"Discount is retrieved for Catalog Code: {discount.CatalogCode} with Amount {discount.Amount}");
    
        var couponModel = _mapper.Map<DiscountDetailModel>(discount);
        return couponModel;
    }
    
    public override async Task<DiscountDetailModel> CreateDiscount(CreateCouponRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<CreateDiscountRequestBody>(request);
        var result = await _discountService.CreateDiscountAsync(requestBody, default);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed created.");
        }
    
        _logger.LogInformation($"Discount is successfully created. Catalog Code: {requestBody.CatalogCode}");
        var couponModel = _mapper.Map<DiscountDetailModel>(requestBody); 
    
        return couponModel;
    }
    
    public override async Task<DiscountDetailModel> UpdateDiscount(UpdateCouponRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<UpdateDiscountRequestBody>(request);
        
        var result = await _discountService.UpdateDiscountAsync(requestBody, default);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed updated.");
        }
    
        _logger.LogInformation($"Discount is successfully updated. Catalog Code: {requestBody.CatalogCode}");
        var couponModel = _mapper.Map<DiscountDetailModel>(requestBody);
    
        return couponModel;
    }
    
    public override async Task<DeleteDiscountResponse> InactiveDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = await _discountService.InactiveDiscountAsync(request.Id);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed deleted.");
        }
    
        _logger.LogInformation($"Discount is successfully deleted. Discount Id: {request.Id}");
    
        var response = new DeleteDiscountResponse()
        {
            Success = true
        };
    
        return response;
    }
}
