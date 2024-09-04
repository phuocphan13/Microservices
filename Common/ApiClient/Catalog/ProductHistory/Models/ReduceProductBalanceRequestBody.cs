namespace ApiClient.Catalog.ProductHistory.Models;

public class ReduceProductBalanceRequestBody
{
    public string ProductCode { get; set; } = null!;
    public int Quantity { get; set; }
}