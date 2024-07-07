using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discount.Domain.Entities;

public class Coupon : ExtendEntity
{
    [Required]
    public string Name { get; set; } = null!;
    
    [MaxLength(256)]
    public string? Description { get; set; }
    
    [Required]
    public CouponEnum Type { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(11, 2)")]
    public decimal Value { get; set; }

    [Required] 
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}