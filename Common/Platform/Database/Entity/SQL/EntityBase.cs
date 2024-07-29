namespace Platform.Database.Entity.SQL;

public class EntityBase : BaseIdEntity
{
    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }
}