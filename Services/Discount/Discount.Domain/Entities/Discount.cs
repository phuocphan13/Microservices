using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class Discount : ExtendEntity
{
    [Required] 
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    [Required] 
    public string? CouponId { get; set; }
}