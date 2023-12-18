using System.ComponentModel.DataAnnotations;

namespace Discount.Domain.Entities;

public class ExtendEntity : BaseEntity
{
    [Required] 
    public string? CreatedBy { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
    
    public string? UpdatedBy { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
}