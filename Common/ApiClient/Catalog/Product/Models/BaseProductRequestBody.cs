namespace ApiClient.Catalog.Product.Models;

public class BaseProductRequestBody 
{
    public string? Name { get; set; }

    public string? CategoryId { get; set; }

    public string? SubCategoryId { get; set; }

    public string? Description { get; set; }

    public string? Summary { get; set; }

    public string? ImageFile { get; set; }

    public decimal Price { get; set; }
}