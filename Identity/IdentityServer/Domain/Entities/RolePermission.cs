using IdentityServer.Domain.Entities.Enums;
using Platform.Database.Entity.SQL;

namespace IdentityServer.Domain.Entities;

public class RolePermission : EntityBase
{
    public Guid ExternalId { get; init; } = Guid.NewGuid();
    
    public Guid RoleId { get; set; }
    
    public Guid ApplicationId { get; set; }
    
    public Guid FeatureId { get; set; }
    
    public PermissionState State { get; set; }
}