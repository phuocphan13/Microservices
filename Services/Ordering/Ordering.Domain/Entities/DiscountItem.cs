using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace Ordering.Domain.Entities;

public class DiscountItem : BaseIdEntity
{
    public DiscountEnum Type { get; set; }

    [MaxLength(256)]
    public string CatalogCode { get; set; } = null!;

    public decimal Amount { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}