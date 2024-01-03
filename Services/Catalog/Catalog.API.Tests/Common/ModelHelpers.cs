using ApiClient.Catalog.Models.Catalog.Category;
using ApiClient.Catalog.Product.Models;
using ApiClient.Catalog.SubCategory.Models;
using Catalog.API.Extensions;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.Common;

public static class ModelHelpers
{
    public static class Product
    {
        public static List<ProductSummary> GenerateProductSummaries(int number = 2, string categoryName = null!, string subCategoryName = null!, Action<ProductSummary>? initAction = default)
        {
            categoryName = string.IsNullOrWhiteSpace(categoryName) ? CommonHelpers.GenerateRandomString() : categoryName;
            subCategoryName = string.IsNullOrWhiteSpace(subCategoryName) ? CommonHelpers.GenerateRandomString() : subCategoryName;

            var summaries = new List<ProductSummary>();

            for (int i = 0; i < number; i++)
            {
                var summary = GenerateProductEntity().ToSummary(categoryName, subCategoryName);
                
                initAction?.Invoke(summary);
                
                summaries.Add(summary);
            }

            return summaries;
        }

        public static ProductDetail GenerateProductDetail(string id = null!, Action<ProductDetail>? initAction = default)
        {
            var detail = GenerateProductEntity(id).ToDetail();

            initAction?.Invoke(detail);
            
            return detail;
        }

        private static TRequestBody GenerateBaseRequestBody<TRequestBody>(string category = null!, string subCategory = null!)
            where TRequestBody : BaseProductRequestBody, new()
        {
            return new TRequestBody()
            {
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
                Category = string.IsNullOrWhiteSpace(category) ? CommonHelpers.GenerateRandomString() : category,
                SubCategory = string.IsNullOrWhiteSpace(subCategory) ? CommonHelpers.GenerateRandomString() : subCategory,
            };
        }

        public static CreateProductRequestBody GenerateCreateRequestBody(string category = null!, string subCategory = null!, Action<CreateProductRequestBody>? initAction = default)
        {
            var requestBody = GenerateBaseRequestBody<CreateProductRequestBody>(category, subCategory);

            initAction?.Invoke(requestBody);
            
            return requestBody;
        }

        public static UpdateProductRequestBody GenerateUpdateRequestBody(string id = null!, string name = null!, Action<UpdateProductRequestBody>? initAction = default)
        {
            var requestBody = GenerateBaseRequestBody<UpdateProductRequestBody>();
            requestBody.Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id;
            requestBody.Name = string.IsNullOrWhiteSpace(name) ? CommonHelpers.GenerateRandomString() : name;

            initAction?.Invoke(requestBody);

            return requestBody;
        }

        public static List<Entities.Product> GenerateProductEntities(int number = 2, string categoryId = null!, string subCategoryId = null!)
        {
            var products = new List<Entities.Product>();

            categoryId = string.IsNullOrWhiteSpace(categoryId) ? CommonHelpers.GenerateRandomString() : categoryId;
            subCategoryId = string.IsNullOrWhiteSpace(subCategoryId) ? CommonHelpers.GenerateRandomString() : subCategoryId;

            for (int i = 0; i < number; i++)
            {
                products.Add(GenerateProductEntity(categoryId: categoryId, subCategoryId: subCategoryId));
            }

            return products;
        }

        public static Entities.Product GenerateProductEntity(string id = null!, string categoryId = null!, string subCategoryId = null!, Action<Entities.Product>? initAction = default)
        {
            var entity = new Entities.Product()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                ProductCode = CommonHelpers.GenerateRandomString(),
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
                CategoryId = string.IsNullOrWhiteSpace(categoryId) ? CommonHelpers.GenerateBsonId() : categoryId,
                SubCategoryId = string.IsNullOrWhiteSpace(subCategoryId) ? CommonHelpers.GenerateBsonId() : subCategoryId,
            };
            
            initAction?.Invoke(entity);

            return entity;
        }
    }

    public static class Category
    {
        public static List<CategorySummary> GenerateCategorySummaries(int number = 2)
        {
            var summaries = new List<CategorySummary>();

            for (int i = 0; i < number; i++)
            {
                summaries.Add(GenerateCategory().ToSummary());
            }

            return summaries;
        }

        public static List<Entities.Category> GenerateCategories(int number = 2)
        {
            var categories = new List<Entities.Category>();

            for (int i = 0; i < number; i++)
            {
                categories.Add(GenerateCategory());
            }

            return categories;
        }

        public static Entities.Category GenerateCategory(string id = null!)
        {
            return new Entities.Category()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                CategoryCode = CommonHelpers.GenerateRandomString(),
                Description = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
            };
        }

        private static TRequestBody GenerateBaseRequestBody<TRequestBody>()
            where TRequestBody : BaseCategoryRequestBody, new()
        {
            return new TRequestBody()
            {
                Description = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                CategoryCode = CommonHelpers.GenerateRandomString(),
            };
        }

        public static CreateCategoryRequestBody GenerateCreateRequestBody(Action<CreateCategoryRequestBody>? initAction = default)
        {
            var requestBody = GenerateBaseRequestBody<CreateCategoryRequestBody>();

            initAction?.Invoke(requestBody);

            return requestBody;
        }

        public static UpdateCategoryRequestBody GenerateUpdateRequestBody(string id = null!, string name = null!, Action<UpdateCategoryRequestBody>? initAction = default)
        {
            var requestBody = GenerateBaseRequestBody<UpdateCategoryRequestBody>();
            requestBody.Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id;
            requestBody.Name = string.IsNullOrWhiteSpace(name) ? CommonHelpers.GenerateRandomString() : name;

            initAction?.Invoke(requestBody);

            return requestBody;
        }
    }

    public static class SubCategory
    {
        public static List<SubCategorySummary> GenerateSubCategorySummaries(int number = 2)
        {
            var summaries = new List<SubCategorySummary>();

            for (int i = 0; i < number; i++)
            {
                summaries.Add(GenerateSubCategory().ToSummary());
            }

            return summaries;
        }

        public static List<Entities.SubCategory> GenerateSubCategories(int number = 2)
        {
            var categories = new List<Entities.SubCategory>();

            for (int i = 0; i < number; i++)
            {
                categories.Add(GenerateSubCategory());
            }

            return categories;
        }

        public static Entities.SubCategory GenerateSubCategory(string id = null!, string categoryId = null!)
        {
            return new Entities.SubCategory()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                SubCategoryCode = CommonHelpers.GenerateRandomString(),
                Description = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                CategoryId = string.IsNullOrWhiteSpace(categoryId) ? CommonHelpers.GenerateBsonId() : categoryId
            };
        }
    }
}