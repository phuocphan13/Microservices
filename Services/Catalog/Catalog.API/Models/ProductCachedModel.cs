namespace Catalog.API.Models;

public class ProductCachedModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ProductCode { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Balance { get; set; }
    public string? CategoryId { get; set; }
    public string? SubCategoryId { get; set; }
    public bool HasChange { get; set; }
}