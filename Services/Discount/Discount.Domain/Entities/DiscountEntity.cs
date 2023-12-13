using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class DiscountEntity : ExtendEntity
{
    [Required] 
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    [Required] 
    public string? CouponId { get; set; }
}