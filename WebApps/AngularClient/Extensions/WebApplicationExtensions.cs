using ApiClient.Discount.Models.Coupon;
using Coupon.Grpc.Protos;

namespace AngularClient.Extensions;

static class WebApplicationExtensions 
{
    public static List<ApiClient.Discount.Models.Coupon.CouponSummary> ToSummaries(this CouponListResponse response)
    {
        return response.CouponList.Select(x => new ApiClient.Discount.Models.Coupon.CouponSummary()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Value = decimal.Parse(x.Value),
            Type = (int)x.Type,
            FromDate = DateTime.Parse(x.FromDate),
            ToDate = string.IsNullOrEmpty(x.ToDate) ? (DateTime?)null : DateTime.Parse(x.ToDate),
            IsActive = x.IsActive,
        }).ToList();
    }
}