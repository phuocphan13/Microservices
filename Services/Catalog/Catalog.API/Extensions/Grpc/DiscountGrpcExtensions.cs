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
            CatalogCode = model.CatalogName
        };
    }
}