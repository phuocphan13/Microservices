using System.ComponentModel;
using ApiClient.Basket.Events;
using ApiClient.Catalog.ProductHistory.Models;
using ApiClient.Common.MessageQueue;

namespace ApiClient.Catalog.Product.Events;

[Description(EventBusConstants.OrderProccess.Accepted)]
public class ProductBalanceUpdateMessage : BaseOrderingMessage
{
    public string ReceiptNumber { get; set; } = null!;

    public IEnumerable<ReduceProductBalanceRequestBody>? Products { get; set; }
}