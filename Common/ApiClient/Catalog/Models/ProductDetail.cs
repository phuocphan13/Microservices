namespace ApiClient.Catalog.Models;

public class ProductDetail : ProductSummary
{
    public string? Summary { get; set; }
    
    public string? ImageFile { get; set; }

    public decimal Price { get; set; }
}