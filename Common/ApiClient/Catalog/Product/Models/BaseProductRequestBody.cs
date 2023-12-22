using ApiClient.Common.Models;

namespace ApiClient.Catalog.Product.Models;

public class BaseProductRequestBody : BaseRequestBody
{
    public string? Name { get; set; }

    public string? Category { get; set; }
    public string? SubCategory { get; set; }

    public string? Description { get; set; }

    public string? Summary { get; set; }

    public string? ImageFile { get; set; }

    public decimal Price { get; set; }
}