using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using AutoMapper;
using Discount.Domain.Extensions;
using Discount.Domain.Services;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.GrpcServices;

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
    
        var discountModel = _mapper.Map<DiscountDetailModel>(discount);
        return discountModel;
    }

    public override async Task<ListDetailsModel> GetListDiscounts(GetListDiscountRequest request, ServerCallContext context)
    {
        if (request.Type == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Type cannot be null."));
        }

        if (!request.CatalogCodes.Any())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Catalog Code cannot be null."));
        }

        var discounts = await _discountService.GetListDiscountsByCatalogCodeAsync((DiscountEnum)request.Type, request.CatalogCodes.ToList());

        if (discounts is null || !discounts.Any())
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Type = {request.Type} is not existed"));
        }

        var model = new ListDetailsModel();

        foreach (var dc in discounts)
        {
            var discountModel = _mapper.Map<DiscountDetailModel>(dc);
            model.Items.Add(discountModel);
        }
        
        return model;
    }

    public override async Task<DiscountDetailModel?> GetDiscountByCatalogCode(GetDiscountByCatalogCodeRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.CatalogCode))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Id cannot be null."));
        }

        var discount = await _discountService.GetDiscountByCatalogCodeAsync(request.Type, request.CatalogCode);

        if (discount is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Code = {request.CatalogCode} is not existed"));
        }

        _logger.LogInformation($"Discount is retrieved for Catalog Code: {discount.CatalogCode} with Amount {discount.Amount}");

        var couponModel = _mapper.Map<DiscountDetailModel>(discount);
        return couponModel;
    }
    
    public override async Task<DiscountDetailModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<CreateDiscountRequestBody>(request);
        var result = await _discountService.CreateDiscountAsync(requestBody, default);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed created.");
        }
    
        _logger.LogInformation($"Discount is successfully created. Catalog Code: {requestBody.CatalogCode}");
        var couponModel = _mapper.Map<DiscountDetailModel>(result); 
    
        return couponModel;
    }
    
    public override async Task<DiscountDetailModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var requestBody = _mapper.Map<UpdateDiscountRequestBody>(request);
        
        var result = await _discountService.UpdateDiscountAsync(requestBody, default);
        
        if (result is null)
        {
            _logger.LogError("Discount is failed updated.");
        }
    
        _logger.LogInformation($"Discount is successfully updated. Catalog Code: {requestBody.CatalogCode}");
        var couponModel = _mapper.Map<DiscountDetailModel>(result);
    
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

    public override async Task<AmountAfterDiscountResponse> TotalDiscountAmount(ListCodeRequest request, ServerCallContext context)
    {
        if (request is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Cannot be null."));
        }

        if (!request.Codes.Any())
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Cannot be null."));
        }

        if (request.Codes.Any(x => string.IsNullOrWhiteSpace(x)))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Cannot be null."));
        }

        var requestBody = _mapper.Map<List<CombinationCodeRequestBody>>(request);

        var amounts = await _discountService.TotalDiscountAmountAsync(requestBody);

        if (amounts is null || !amounts.Any())
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Code = {request.Codes} is not existed"));
        }

        return amounts.ToAmountAfterDiscountResponse();
    }
}
