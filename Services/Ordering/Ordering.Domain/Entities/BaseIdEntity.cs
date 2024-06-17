using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Entities;

public class BaseIdEntity
{
    [Key]
    public int Id { get; protected set; }
}