using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using Catalog.API.Entities;
using Catalog.API.Extensions.Grpc;
using Discount.Grpc.Protos;

namespace Catalog.API.Services.Grpc;

public interface IDiscountGrpcService
{
    Task<DiscountDetail> GetDiscount(string productName);
    Task<DiscountDetail> GetDiscountByCatalogCode(DiscountEnum type, string catalogCode);
    Task<List<DiscountDetail>> GetListDiscountsByCatalogCodeAsync(DiscountEnum type, IEnumerable<string> catalogCodes);
    Task<AmountAfterDiscountResponse> GetAmountsAfterDiscountAsync(List<Category> entityCategory, List<SubCategory> entitySubCategory, List<Product> entityProduct);
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

    public async Task<AmountAfterDiscountResponse> GetAmountsAfterDiscountAsync(List<Category> entityCategory, List<SubCategory> entitySubCategory, List<Product> entityProduct)
    {
        var listCategory = new AmountAfterDiscountRequest();

       foreach(var categoryItem in entityCategory)
        {
            var category = new ListCategoryModel();

            category.Type = "2";
            category.CatalogCode = categoryItem.CategoryCode;

            foreach(var subCategoryItem in entitySubCategory.Where(x=>x.CategoryId == categoryItem.Id))
            {
                var subCategory = new ListSubCategoryModel();

                subCategory.SubType = "3";
                subCategory.SubCatalogCode = subCategoryItem.SubCategoryCode;

                foreach(var productItem in entityProduct.Where(x=>x.SubCategoryId == subCategoryItem.Id))
                {
                    var product = new ListProductModel();

                    product.ProdType = "4";
                    product.ProdCatalogCode = productItem.ProductCode;

                    subCategory.ProdList.Add(product);
                }

                category.SubList.Add(subCategory);
            }

            listCategory.Categories.Add(category);
        }

        var result = await _discountGrpcService.AmountAfterDiscountAsync(listCategory);

        return result;
    }

}