using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace IdentityServer.Domain.Entities;

public class Application : EntityBase
{
    public bool IsActive { get; set; }
    
    public Guid ExternalId { get; init; }

    [MaxLength(256)]
    public string Name { get; set; } = null!;

    [MaxLength(512)]
    public string Description { get; set; } = null!;
    
    public List<Feature> Features { get; set; } = new();
    
    
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}