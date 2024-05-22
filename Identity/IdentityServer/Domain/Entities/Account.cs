using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Entities;

public class Account : IdentityUser<Guid>
{
    public bool IsActive { get; set; }

    [Required]
    public DateTime CreatedDate { get; init; }

    public DateTime? LastLogin { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullName { get; set; } = null!;

    public string? Mobile { get; set; }

    public DateTime? Birthday { get; set; }


    public int? Sex { get; set; }
}