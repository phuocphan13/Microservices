namespace ApiClient.Catalog.Product.Models;


public class ProductDetail : ProductSummary
{
    public string? Summary { get; set; }

    public string? ImageFile { get; set; }
    
    public string? CategoryId { get; set; }
    
    public string? SubCategoryId { get; set; }
}