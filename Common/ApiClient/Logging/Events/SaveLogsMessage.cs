using ApiClient.Basket.Events;
using ApiClient.Logging.Models;

namespace ApiClient.Logging.Events;

public class SaveLogsMessage : BaseMessage
{
    public string CorrelationId { get; set; } = null!;

    public ICollection<CreateLogRequestBody> RequestBodies { get; set; } = [];
}