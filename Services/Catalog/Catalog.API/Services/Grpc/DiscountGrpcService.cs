using ApiClient.Discount.Models.Discount;
using Catalog.API.Entities;
using Catalog.API.Extensions.Grpc;
using Catalog.API.Models;
using Discount.Grpc.Protos;

namespace Catalog.API.Services.Grpc;

public interface IDiscountGrpcService
{
    Task<DiscountDetail> GetDiscount(string productName);
    Task<DiscountDetail> GetDiscountByCatalogCode(DiscountEnum type, string catalogCode);
    Task<List<DiscountDetail>> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, IEnumerable<string> catalogCodes);
    Task<List<DiscountSummary>> GetAmountsAfterDiscountAsync(List<Category> entityCategory, List<SubCategory> entitySubCategory, List<ProductCachedModel> entityProduct);
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
        catch (Exception)
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

    public async Task<List<DiscountSummary>> GetAmountsAfterDiscountAsync(List<Category> entityCategory, List<SubCategory> entitySubCategory, List<ProductCachedModel> entityProduct)
    {
        if (entityCategory is null || entityCategory.Count == 0) {
            throw new ArgumentException("Category list cannot be null or empty");
        }

        if (entitySubCategory is null || entitySubCategory.Count == 0)
        {
            throw new ArgumentException("Sub-Category list cannot be null or empty");
        }

        if (entityProduct is null || entityProduct.Count == 0)
        {
            throw new ArgumentException("Product list cannot be null or empty");
        }

        var request = new ListCodeRequest();
        
        foreach (var categoryItem in entityCategory)
        {
            foreach (var subCategoryItem in entitySubCategory.Where(x => x.CategoryId == categoryItem.Id))
            {
                foreach (var productItem in entityProduct.Where(x => x.SubCategoryId == subCategoryItem.Id))
                {
                    var combinatedCode = $"{productItem.Code}.{subCategoryItem.SubCategoryCode}.{categoryItem.CategoryCode}";

                    request.Codes.Add(combinatedCode);
                }
            }
        }

        AmountAfterDiscountResponse? result;

        try
        {
            result = await _discountGrpcService.TotalDiscountAmountAsync(request);
        }
        catch
        {
            return new();
        }
        

        return result.ToListDetail();
    }
}