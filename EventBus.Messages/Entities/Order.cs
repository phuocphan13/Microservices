using System.ComponentModel.DataAnnotations;

namespace EventBus.Messages.Entities;

public class Order
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string UserId { get; set; } = null!;

    [MaxLength(128)]
    public string UserName { get; set; } = null!;
    
    [MaxLength(36)]
    public string ReceiptNumber { get; set; } = null!;

    public DateTime Timestamp { get; set; }
    
    [MaxLength(256)]
    public string? Description { get; set; }

    public ProcessEnum Proccess { get; set; }

    [MaxLength(64)]
    public string MemberId { get; set; } = null!;

    [MaxLength(64)]
    public string EventId { get; set; } = null!;
}