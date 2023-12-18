using ApiClient.Common.Models;

namespace ApiClient.Catalog.Models.Catalog.Category;

public class BaseCategoryRequestBody : BaseRequestBody
{
    public string? Name { get; set; }

    public string? CategoryCode { get; set; }

    public string? Description { get; set; }
}
