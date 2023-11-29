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
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
        };
    }

    public static ProductDetail ToDetail(this Product product)
    {
        return new ProductDetail()
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            Price = product.Price,
            Summary = product.Summary,
            ImageFile = product.ImageFile
        };
    }

    public static Product ToCreateProduct(this CreateProductRequestBody requestBody)
    {
        return new Product()
        {
            Name = requestBody.Name,
            Category = requestBody.Category,
            Description = requestBody.Description,
            Price = requestBody.Price,
            Summary = requestBody.Summary,
            ImageFile = requestBody.ImageFile
        };
    }

    public static void ToUpdateProduct(this Product product, UpdateProductRequestBody requestBody)
    {
        product.Id = requestBody.Id;
        product.Name = requestBody.Name;
        product.Category = requestBody.Category;
        product.Description = requestBody.Description;
        product.Price = requestBody.Price;
        product.Summary = requestBody.Summary;
        product.ImageFile = requestBody.ImageFile;
    }
}