using ApiClient.Basket.Events;
using ApiClient.Catalog.ProductHistory.Models;

namespace ApiClient.Catalog.Product.Events;

public class ProductBalanceUpdateMessage : BaseMessage
{
    public string ReceiptNumber { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string MemberId { get; set; }
    public string EventId { get; set; }

    public IEnumerable<ReduceProductBalanceRequestBody>? Products { get; set; }
}