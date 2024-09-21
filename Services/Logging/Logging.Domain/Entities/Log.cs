using Logging.Domain.Enums;

namespace Logging.Domain.Entities;

public sealed class Log
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public string CreatedBy { get; set; } = null!;
    
    public LogType Type { get; set; }
    
    public LogMeter Meter { get; set; }
    
    public string ObjectName { get; set; } = null!;
}