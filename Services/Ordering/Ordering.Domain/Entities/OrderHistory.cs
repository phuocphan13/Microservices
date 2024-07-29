using Platform.Database.Entity.SQL;

namespace Ordering.Domain.Entities;

public class OrderHistory : BaseIdEntity
{
    public DateTime CreatedDate { get; set; }
    
    public OrderStatus LastStatus { get; set; }
    public OrderStatus CurrentStatus { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}