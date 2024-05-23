using Catalog.API.Common.Helpers;
using Catalog.API.Entities;
using MongoDB.Driver;
using Platform.Constants;

namespace Catalog.API.Common.Extensions;

public static class InitializeDB
{
    private static readonly string CategorySmartPhoneId = ModelHelpers.GenerateId();
    private static readonly string CategoryLaptopPhoneId = ModelHelpers.GenerateId();
    private static readonly string CategoryHeadphoneId = ModelHelpers.GenerateId();

    private static readonly string SubCategoryAppleId = ModelHelpers.GenerateId();
    private static readonly string SubCategorySamSungId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryHauweiId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryXiaomiId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryHTCId = ModelHelpers.GenerateId();
    private static readonly string SubCategoryLGId = ModelHelpers.GenerateId();
    
    public static async Task InitializePlatformDbContextsAsync(
        this IApplicationBuilder builder,
        ConfigurationManager configuration,
        bool isRebuildSchema = true)
    {
        var client = new MongoClient(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.ConnectionString));
        var database = client.GetDatabase(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.DatabaseName));

        if (isRebuildSchema)
        {
            var cateCollection = database.GetCollection<Category>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Category);
            cateCollection.DeleteMany(Builders<Category>.Filter.Empty);
            await cateCollection.InsertManyAsync(GetPreconfiguratedCategories());

            var subCateCollection = database.GetCollection<SubCategory>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.SubCategory);
            subCateCollection.DeleteMany(Builders<SubCategory>.Filter.Empty);
            await subCateCollection.InsertManyAsync(GetPreconfiguratedSubCategories());

            var productCollection = database.GetCollection<Product>(DatabaseConst.ConnectionSetting.MongoDB.CollectionName.Product);
            productCollection.DeleteMany(Builders<Product>.Filter.Empty);
            await productCollection.InsertManyAsync(GetPreconfiguratedProducts());
        }
    }
    
    private static IEnumerable<Category> GetPreconfiguratedCategories()
    {
        return new List<Category>()
        {
            new()
            {
                Id = CategorySmartPhoneId,
                CategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Smart Phone",
                Description = "Smart Phone Description"
            },
            new()
            {
                Id = CategoryLaptopPhoneId,
                CategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Laptop",
                Description = "Laptop Description"
            },
            new()
            {
                Id = CategoryHeadphoneId,
                CategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Head Phone",
                Description = "Laptop Description"
            }
        };
    }

    private static IEnumerable<SubCategory> GetPreconfiguratedSubCategories()
    {
        return new List<SubCategory>()
        {
            new()
            {
                Id = SubCategoryAppleId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Apple",
                Description = "Apple Description",
                CategoryId = CategorySmartPhoneId
            },
            new()
            {
                Id = SubCategorySamSungId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "SamSung",
                Description = "SamSung Description",
                CategoryId = CategorySmartPhoneId
            },
            new()
            {
                Id = SubCategoryHauweiId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Huawei",
                Description = "Huawei Description",
                CategoryId = CategorySmartPhoneId
            },
            new()
            {
                Id = SubCategoryXiaomiId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "Xiaomi",
                Description = "Xiaomi Description",
                CategoryId = CategorySmartPhoneId
            },
            new()
            {
                Id = SubCategoryHTCId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "HTC",
                Description = "HTC Description",
                CategoryId = CategorySmartPhoneId
            },
            new()
            {
                Id = SubCategoryLGId,
                SubCategoryCode = ModelHelpers.GenerateGuid(),
                Name = "LG",
                Description = "LG Description",
                CategoryId = CategorySmartPhoneId
            },
        };
    }

    private static IEnumerable<Product> GetPreconfiguratedProducts()
    {
        return new List<Product>()
        {
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = "IPX-APL",
                Name = "IPhone X",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-1.png",
                Price = 950.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategoryAppleId
            },
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = ModelHelpers.GenerateGuid(),
                Name = "Samsung 10",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-2.png",
                Price = 840.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategorySamSungId
            },
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = ModelHelpers.GenerateGuid(),
                Name = "Huawei Plus",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-3.png",
                Price = 650.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategoryHauweiId
            },
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = ModelHelpers.GenerateGuid(),
                Name = "Xiaomi Mi 9",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-4.png",
                Price = 470.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategoryXiaomiId
            },
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = ModelHelpers.GenerateGuid(),
                Name = "HTC U11+ Plus",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-5.png",
                Price = 380.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategoryHTCId
            },
            new()
            {
                Id = ModelHelpers.GenerateId(),
                ProductCode = ModelHelpers.GenerateGuid(),
                Name = "LG G7 ThinQ",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                CategoryId = CategorySmartPhoneId,
                SubCategoryId = SubCategoryLGId
            }
        };
    }
}