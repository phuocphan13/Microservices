using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace IdentityServer.Domain.Entities;

public class Feature : EntityBase
{
    public Guid ExternalId { get; init; }

    [MaxLength(256)]
    public string Name { get; set; } = null!;

    [MaxLength(512)]
    public string Description { get; set; } = null!;
    
    public int ApplicationId { get; set; }
    public Application Application { get; set; } = null!;
}