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
                Description = "SamSung 10 Description",
                Type = DiscountEnum.Product,
                CatalogCode = "IPX-APL",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                IsActive = true,
            }
        };
    }
}