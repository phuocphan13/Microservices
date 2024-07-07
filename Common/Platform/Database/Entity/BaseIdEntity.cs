using System.ComponentModel.DataAnnotations;

namespace Platform.Database.Entity;

public class BaseIdEntity
{
    [Key]
    public int Id { get; protected set; }
}