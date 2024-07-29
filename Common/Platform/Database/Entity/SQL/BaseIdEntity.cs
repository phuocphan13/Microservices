using System.ComponentModel.DataAnnotations;

namespace Platform.Database.Entity.SQL;

public class BaseIdEntity
{
    [Key]
    public int Id { get; protected set; }
}