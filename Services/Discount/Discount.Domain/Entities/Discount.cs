using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class Discount : ExtendEntity
{
    [MaxLength(256)]
    public string? Description { get; set; }

    [Required] 
    public int Amount { get; set; }
    
    [Required]
    public DiscountEnum Type { get; set; }

    [MaxLength(32)]
    public string? CatalogCode { get; set; }

    [Required] 
    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}