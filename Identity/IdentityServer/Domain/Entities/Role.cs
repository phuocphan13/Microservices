using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    [Column(TypeName = "nvarchar(128)")]
    public string? Description { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(36)]
    public string? CreatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    [MaxLength(36)]
    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }
    public bool Deleted { get; set; }
}