using ApiClient.Catalog.Models.Catalog.Product;
using UnitTest.Common.Helpers;

namespace Catalog.API.Tests.Common;

public static class ModelHelpers
{
    public static class Product
    {
        public static List<ProductSummary> GenerateProductSummaries()
        {
            return new List<ProductSummary>()
            {
                new()
                {
                    Id = CommonHelpers.GenerateBsonId(),
                    Category = CommonHelpers.GenerateRandomString(),
                    Description = CommonHelpers.GenerateRandomString(),
                    ImageFile = CommonHelpers.GenerateRandomString(),
                    Name = CommonHelpers.GenerateRandomString(),
                    SubCategory = CommonHelpers.GenerateRandomString(),
                    Price = CommonHelpers.GenerateRandomDecimal()
                },
                new()
                {
                    Id = CommonHelpers.GenerateBsonId(),
                    Category = CommonHelpers.GenerateRandomString(),
                    Description = CommonHelpers.GenerateRandomString(),
                    ImageFile = CommonHelpers.GenerateRandomString(),
                    Name = CommonHelpers.GenerateRandomString(),
                    SubCategory = CommonHelpers.GenerateRandomString(),
                    Price = CommonHelpers.GenerateRandomDecimal(),
                }
            };
        }

        public static ProductDetail GenerateProductDetail(string id = null!)
        {
            return new ProductDetail()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                Category = CommonHelpers.GenerateRandomString(),
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                SubCategory = CommonHelpers.GenerateRandomString(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
            };
        }

        public static CreateProductRequestBody GenerateCreateRequestBody()
        {
            return new CreateProductRequestBody()
            {
                Category = CommonHelpers.GenerateRandomString(),
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                SubCategory = CommonHelpers.GenerateRandomString(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
            };
        }

        public static UpdateProductRequestBody GenerateUpdateRequestBody(string id = null!, string name = null!)
        {
            return new UpdateProductRequestBody()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                Category = string.IsNullOrWhiteSpace(name) ? CommonHelpers.GenerateRandomString() : name,
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                SubCategory = CommonHelpers.GenerateRandomString(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
            };
        }

        public static Entities.Product GenerateProductEntity(string id = null!)
        {
            return new Entities.Product()
            {
                Id = string.IsNullOrWhiteSpace(id) ? CommonHelpers.GenerateBsonId() : id,
                CategoryId = CommonHelpers.GenerateBsonId(),
                Description = CommonHelpers.GenerateRandomString(),
                ImageFile = CommonHelpers.GenerateRandomString(),
                Name = CommonHelpers.GenerateRandomString(),
                SubCategoryId = CommonHelpers.GenerateBsonId(),
                Price = CommonHelpers.GenerateRandomDecimal(),
                Summary = CommonHelpers.GenerateRandomString(),
            };
        }
    }
}