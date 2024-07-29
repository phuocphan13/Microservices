using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace Worker.Entities;

public class Job : EntityBase
{
    public Guid ExternalId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}