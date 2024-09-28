using System.ComponentModel;
using ApiClient.Basket.Events;
using ApiClient.Common.MessageQueue;

namespace ApiClient.Ordering.Events;

[Description(EventBusConstants.OrderProccess.Failed)]
public class FailureOrderMessage : BaseOrderingMessage
{
    public string ReceiptNumber { get; set; } = null!;
}