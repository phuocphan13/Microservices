using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Entities;

public class Order : EntityBase
{
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    [MaxLength(256)]
    public string UserName { get; set; } = null!;
    
    public OrderStatus Status { get; set; }

    public decimal TotalPrice { get; set; }

    [MaxLength(256)]
    public string ClientName { get; set; } = null!;

    [MaxLength(11)]
    public string PhoneNumber { get; set; } = null!;

    [MaxLength(256)]
    public string Email { get; set; } = null!;

    [MaxLength(256)]
    public string Address { get; set; } = null!;

    [MaxLength(256)]
    public string? Description { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}