using System.ComponentModel.DataAnnotations;

namespace EventBus.Messages.Entities;

public class Log
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string CorrelationId { get; set; } = null!;

    [MaxLength(int.MaxValue)]
    public string Text { get; set; } = null!;

    [MaxLength(64)]
    public string EventId { get; set; } = Guid.NewGuid().ToString();

    public int Type { get; set; }
}