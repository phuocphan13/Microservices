using ApiClient.Basket.Events;
using ApiClient.Catalog.ProductHistory.Models;

namespace ApiClient.Catalog.Product.Events;

public class ProductBalanceUpdateMessage : BaseMessage
{
    public string ReceiptNumber { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;

    public IEnumerable<ReduceProductBalanceRequestBody>? Products { get; set; }
}