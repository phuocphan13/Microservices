using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Discount.Models.Coupon;

public class BaseCouponRequestBody
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Type { get; set; }

    public decimal Value { get; set; }
    
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}