using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace Ordering.Domain.Entities;

public class OrderItem : BaseIdEntity
{
    [MaxLength(72)]
    public string? ProductCode { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;
}