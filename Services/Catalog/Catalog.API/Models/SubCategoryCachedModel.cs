using Catalog.API.Models.Cached;

namespace Catalog.API.Models;

public class SubCategoryCachedModel : BaseCachedModel
{
    public string? CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
