using ApiClient.Basket.Events;

namespace ApiClient.Logging.Events;

public class SaveLogsMessage : BaseMessage
{
    public string CorrelationId { get; set; } = null!;
}