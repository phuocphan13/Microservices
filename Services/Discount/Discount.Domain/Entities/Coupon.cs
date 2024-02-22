using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class Coupon : ExtendEntity
{
    [Required]
    public string? Name { get; set; }
    
    [MaxLength(256)]
    public string? Description { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required] 
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}