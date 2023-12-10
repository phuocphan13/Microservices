using Discount.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Domain.Common.InitializeDB;

public static class InitializeDB
{
    public static async Task InitializePlatformDbContextsAsync(
        this IApplicationBuilder builder,
        ConfigurationManager configuration,
        bool isRebuildSchema = false)
    {
        if (isRebuildSchema)
        {
            var connection = new NpgsqlConnection(configuration["DatabaseSettings:ConnectionString"]);
            
            ConfigureDB.DropTable<Coupon>(connection);
            ConfigureDB.CreateTable<Coupon>(connection);
            await ConfigureDB.InsertTable(connection, GenerateCoupons());
        }
    }

    private static List<Coupon> GenerateCoupons()
    {
        return new List<Coupon>()
        {
            new()
            {
                Amount = 200,
                Code = "SamSung 10",
                Description = "SamSung 10 Description",
                Type = CatalogType.Product 
            }
        };
    }
    
    // public static void Initlize(IConfiguration configuration)
    // {
    //     var connection = new NpgsqlConnection(configuration["DatabaseSettings:ConnectionString"]);
    //     ConfigureDB.DropTable<Coupon>(connection);
    //     ConfigureDB.CreateTable<Coupon>(connection);
    // }
}