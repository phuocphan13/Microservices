namespace ApiClient.Catalog.Product.Models;

public class ProductSummary
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }
    
    public string? SubCategory { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }
    
    public string? Code { get; set; }
    
    public int? Balance { get; set; }
}