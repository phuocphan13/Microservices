using ApiClient.Discount.Models.Discount;
using Discount.Grpc.Protos;

namespace Catalog.API.Extensions.Grpc;

public static class DiscountGrpcExtensions
{
    public static DiscountDetail ToDetail(this DiscountDetailModel model)
    {
        return new DiscountDetail()
        {
            Amount = model.Amount,
            Description = model.Description,
            Id = model.Id,
            CatalogCode = model.CatalogName,
            Type = (DiscountEnum)model.Type
        };
    }

    public static List<DiscountDetail> ToListDetail(this AmountAfterDiscountResponse model)
    {
        var listDiscounts = new List<DiscountDetail>();
        var discount = new DiscountDetail();

        foreach(var item in model.AmountDiscountResponse)
        {
            discount.Amount = int.Parse(item.Amount);
            discount.CatalogCode = item.CatalogCode;
            listDiscounts.Add(discount);
        }
        return listDiscounts;
    }
}