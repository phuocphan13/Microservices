using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Discount.Models.Coupon;

public class BaseCouponRequestBody
{
    public string? Code { get; set; }

    public CatalogType Type { get; set; }

    public string? Description { get; set; }

    public int Amount { get; set; }
}