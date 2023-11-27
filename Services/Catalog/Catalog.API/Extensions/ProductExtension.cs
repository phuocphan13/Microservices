using ApiClient.Catalog.Models;
using Catalog.API.Entities;

namespace Catalog.API.Extensions;

public static class ProductExtension
{
    public static ProductSummary ToSummary(this Product product)
    {
        return new ProductSummary()
        {
            Id = product.Id,
            Category = product.Category,
            Description = product.Description,
            Price = product.Price
        };
    }
}