using ApiClient.Catalog.Models.Catalog.Category;
using Catalog.API.Entities;

namespace Catalog.API.Extensions;

public static class CategoryExtension
{
    public static Category ToCreateCategory(this CreateCategoryRequestBody requestBody)
    {
        return new Category()
        {
            Name = requestBody.Name,
            Description = requestBody.Description,
            CategoryCode = requestBody.CategoryCode,
        };
    }

    public static void ToUpdateCategory(this Category category, UpdateCategoryRequestBody requestBody)
    {
        category.Name = requestBody.Name;
        category.Description = requestBody.Description;
        category.CategoryCode = requestBody.CategoryCode;
    }

    public static CategorySummary ToSummary(this Category category)
    {
        return new CategorySummary()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CategoryCode = category.CategoryCode,
        };
    }
}
