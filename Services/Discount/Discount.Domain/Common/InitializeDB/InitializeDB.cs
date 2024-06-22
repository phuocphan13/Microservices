using ApiClient.Discount.Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Platform.Constants;

namespace Discount.Domain.Common.InitializeDB;

public static class InitializeDB
{
    public static async Task InitializeDiscountDbContextsAsync(
        this IApplicationBuilder builder,
        ConfigurationManager configuration,
        bool isRebuildSchema = true)
    {
        if (isRebuildSchema)
        {
            var connection = new NpgsqlConnection(configuration[DatabaseConst.ConnectionSetting.Postgres.ConnectionString]);
            
            await ConfigureDB.DropTable<Entities.Coupon>(connection);
            await ConfigureDB.CreateTable<Entities.Coupon>(connection);
            await ConfigureDB.InsertTable(connection, GenerateCoupons());

            await ConfigureDB.DropTable<Entities.Discount>(connection);
            await ConfigureDB.CreateTable<Entities.Discount>(connection);
            await ConfigureDB.InsertTable(connection, GenerateDiscounts());
        }
    }

    private static List<Entities.Coupon> GenerateCoupons()
    {
        return new List<Entities.Coupon>()
        {
            new()
            {
                Type = CouponEnum.Percent,
                Value = 20,
                Name = "SamSung 10",
                Description = "SamSung 10 Description",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
        };
    }

    private static List<Entities.Discount> GenerateDiscounts()
    {
        return new List<Entities.Discount>()
        {
            new()
            {
                Amount = 200,
                Description = "Phone",
                Type = DiscountEnum.Category,
                CatalogCode = "CATE-CODE-1",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 100,
                Description = "SamSung",
                Type = DiscountEnum.SubCategory,
                CatalogCode = "SUP-CODE-1",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 300,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "SS-10",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 300,
                Description = "SamSung 9 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "SS-09",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 250,
                Description = "MacBook",
                Type = DiscountEnum.Category,
                CatalogCode = "CATE-CODE-2",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 500,
                Description = "MacOS",
                Type = DiscountEnum.SubCategory,
                CatalogCode = "SUP-CODE-2",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 500,
                Description = "MacOS 01",
                Type = DiscountEnum.Product,
                CatalogCode = "Mac-01",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
        };
    }
}