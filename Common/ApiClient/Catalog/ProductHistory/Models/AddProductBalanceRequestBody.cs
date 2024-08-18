namespace ApiClient.Catalog.ProductHistory.Models;

public class AddProductBalanceRequestBody
{
    public string Id { get; set; } = null!;
    
    public int Balance { get; set; }
}