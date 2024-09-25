using System.ComponentModel;
using ApiClient.Basket.Events;
using ApiClient.Common.MessageQueue;

namespace ApiClient.Logging.Events;

public class SaveLogsMessage : BaseMessage
{
    public string CorrelationId { get; set; } = null!;
}