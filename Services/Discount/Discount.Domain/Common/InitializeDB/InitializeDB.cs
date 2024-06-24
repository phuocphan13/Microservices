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
        bool isRebuildSchema = false)
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
                Name = "Happy Monday",
                Description = "Happy Monday Description",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Type = CouponEnum.Percent,
                Value = 20,
                Name = "Happy Tuesday",
                Description = "Happy Tuesday Description",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Type = CouponEnum.Percent,
                Value = 20,
                Name = "Happy Wednesday",
                Description = "Happy Wednesday Description",
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
                Amount = 10,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Category,
                CatalogCode = "CategoryCode-1",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 20,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "CategoryCode-2",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 10,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.SubCategory,
                CatalogCode = "CategoryCode-1",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 20,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.SubCategory,
                CatalogCode = "CategoryCode-2",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 100,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "ProductCode-1",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            },
            new()
            {
                Amount = 200,
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "ProductCode-2",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
        };
    }
}