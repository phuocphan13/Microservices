using ApiClient.Discount.Models.Discount;
using Catalog.API.Extensions.Grpc;
using Discount.Grpc.Protos;

namespace Catalog.API.Services.Grpc;

public interface IDiscountGrpcService
{
    Task<DiscountDetail> GetDiscount(string productName);
    Task<DiscountDetail> GetDiscountByCatalogCode(DiscountEnum type, string catalogCode);
    Task<List<DiscountDetail>> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, IEnumerable<string> catalogCodes);
}

public class DiscountGrpcService : IDiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcService)
    {
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
    }

    public async Task<DiscountDetail> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest()
        {
            Id = productName
        };

        var couponModel = await _discountGrpcService.GetDiscountAsync(discountRequest);

        return couponModel.ToDetail();
    }

    public async Task<DiscountDetail> GetDiscountByCatalogCode(DiscountEnum type, string catalogCode)
    {
        var discountRequest = new GetDiscountByCatalogCodeRequest()
        {
            Type = (int)type,
            CatalogCode = catalogCode
        };

        var couponModel = await _discountGrpcService.GetDiscountByCatalogCodeAsync(discountRequest);

        return couponModel.ToDetail();
    }

    public async Task<List<DiscountDetail>> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, IEnumerable<string> catalogCodes)
    {
        var request = new GetListDiscountRequest()
        {
            Type = (int)type,
        };

        foreach (var code in catalogCodes)
        {
            request.CatalogCodes.Add(code);
        }

        ListDetailsModel? result;

        try
        {
            result = await _discountGrpcService.GetListDiscountsAsync(request);
        }
        catch (Exception ex)
        {
            return new();
        }

        if (result is null || !result.Items.Any())
        {
            return new();
        }

        var discounts = result.Items.Select(x => new DiscountDetail()
        {
            CatalogCode = x.CatalogName,
            Description = x.Description,
            Amount = x.Amount,
        }).ToList();

        return discounts;
    }
}