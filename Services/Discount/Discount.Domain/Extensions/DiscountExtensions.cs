using System.Globalization;
using ApiClient.Discount.Models.Discount;
using ApiClient.Discount.Models.Discount.AmountModel;
using Discount.Grpc.Protos;
using System.Runtime.CompilerServices;

namespace Discount.Domain.Extensions;

public static class DiscountExtensions
{
    public static DiscountDetail ToDetail(this Entities.Discount entity)
    {
        return new DiscountDetail()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Type = entity.Type,
            CatalogCode = entity.CatalogCode,
            Description = entity.Description,
            FromDate = entity.FromDate,
            ToDate = entity.ToDate
        };
    }

    public static Entities.Discount ToEntityFromCreateBody(this CreateDiscountRequestBody body)
    {
        return new Entities.Discount()
        {
            Type = body.Type!.Value,
            CatalogCode = body.CatalogCode!,
            Amount = body.Amount!.Value,
            Description = body.Description!,
            FromDate = body.FromDate!.Value,
            ToDate = body.ToDate,
            IsActive = true
        };
    }

    public static Entities.Discount ToEntityFromUpdateBody(this UpdateDiscountRequestBody body)
    {
        return new Entities.Discount()
        {
            Id = body.Id!.Value,
            Type = body.Type!.Value,
            CatalogCode = body.CatalogCode!,
            Amount = body.Amount!.Value,
            Description = body.Description!,
            FromDate = body.FromDate!.Value,
            ToDate = body.ToDate,
            IsActive = true
        };
    }

    public static AmountAfterDiscountResponse ToAmountAfterDiscountResponse(this IEnumerable<TotalAmountModel> amounts)
    {
        var response = new AmountAfterDiscountResponse();

        var amountResponses = amounts.Select(x => new DiscountResponse()
        {
            Amount = x.Amount.ToString(CultureInfo.InvariantCulture),
            CatalogCode = x.CatalogCode
        });

        response.AmountDiscountResponse.AddRange(amountResponses);

        return response;
    }
}