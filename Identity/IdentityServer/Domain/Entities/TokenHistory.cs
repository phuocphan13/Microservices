using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Platform.Database.Entity.SQL;

namespace IdentityServer.Domain.Entities;

public class TokenHistory : BaseIdEntity
{
    public Guid ExternalId { get; init; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(36)]
    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string Token { get; set; } = null!;
    
    [Required]
    public TokenTypeEnum Type { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    [Required]
    public DateTime ValidTo { get; set; }

    [Required]
    public Guid AccountId { get; set; }

}