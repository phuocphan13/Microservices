using System.ComponentModel.DataAnnotations;

namespace EventBus.Messages.Entities;

public class Basket
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;
    
    public decimal TotalPrice { get; set; }

    public DateTime Timestamp { get; set; }

    [MaxLength(64)]
    public string MemberId { get; set; } = null!;

    [MaxLength(64)]
    public string EventId { get; set; } = null!;
}