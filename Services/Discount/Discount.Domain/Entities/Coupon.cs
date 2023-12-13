using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class Coupon : ExtendEntity
{
    [Required]
    [MaxLength(50)]
    public string? Code { get; set; }

    [Required]
    public CatalogType Type { get; set; }

    [MaxLength(256)]
    public string? Description { get; set; }

    [Required]
    public int Amount { get; set; }
}