using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Models.Catalog.Product;
using ApiClient.Catalog.Models.SubCategory;
using Catalog.API.Common.Helpers;
using Catalog.API.Entities;
using static Catalog.API.Common.Consts.ResponseMessages;
using SubCategory = Catalog.API.Entities.SubCategory;
using 

namespace Catalog.API.Extensions;

public static class SubCategoryExtention
{
    public static SubCategorySummary ToSummary(this SubCategory subCategory)
    {
        return new SubCategorySummary()
        {
            Id = subCategory.Id,
            Name = subCategory.Name,
            SubCategoryCode = subCategory.SubCategoryCode,
            Description = subCategory.Description,
            CategoryId = subCategory.CategoryId,

        };
    }

    public static SubCategory ToCreateSubCategory(this CreateSubCategoryRequestBody requestBody)
    {
        return new SubCategory()
        {
            Name = requestBody.Name,
            Description = requestBody.Description,
            SubCategoryCode = requestBody.SubCategoryCode,
            CategoryId = requestBody.CategoryId,
        };
    }

    public static void ToUpdateSubCategory(this SubCategory subCategory, UpdateSubCategoryRequestBody requestBody)
    {
        subCategory.Name = requestBody.Name;
        subCategory.Description = requestBody.Description;
        subCategory.SubCategoryCode = requestBody.SubCategoryCode;
        subCategory.CategoryId = requestBody.CategoryId;
    }
}
