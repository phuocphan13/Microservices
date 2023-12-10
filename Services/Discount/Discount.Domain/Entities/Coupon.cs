using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }
    
    public CatalogType Type { get; set; }

    public string? Description { get; set; }

    public int Amount { get; set; }
}