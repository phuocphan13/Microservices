using ApiClient.Discount.Models.Discount;
using Discount.Domain.Entities;

namespace Discount.Domain.Extensions;

public static class DiscountExtensions
{
    public static DiscountDetail ToDetail(this Entities.Discount entity)
    {
        return new DiscountDetail()
        {
            Id = entity.Id
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
}