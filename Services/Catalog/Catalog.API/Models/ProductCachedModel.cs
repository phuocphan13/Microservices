using Catalog.API.Models.Cached;

namespace Catalog.API.Models;

public class ProductCachedModel : BaseCachedModel 
{
    public decimal Price { get; set; }
    public int Balance { get; set; }
    public string? CategoryId { get; set; }
    public string? SubCategoryId { get; set; }
}