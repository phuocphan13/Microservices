using Dapper;
using Discount.Domain.Extensions;
using Npgsql;

namespace Discount.Domain.Common.InitializeDB;

public static class ConfigureDB
{
    public static void DropTable<TEntity>(NpgsqlConnection connection)
    {
        var tableName = typeof(TEntity).Name;
        string query = $"Drop Table {tableName}";
        connection.ExecuteAsync(query);
    }

    public static void CreateTable<TEntity>(NpgsqlConnection connection)
        where TEntity : new()
    {
        var entity = new TEntity();

        var properties = entity.TablePropertiesBuilder();

        if (string.IsNullOrWhiteSpace(properties))
        {
            return;
        }

        var tableName = typeof(TEntity).Name;
        string query = $"Create table {tableName} ({properties})";

        connection.ExecuteAsync(query);
    }

    public static async Task InsertTable<TEntity>(NpgsqlConnection connection, List<TEntity> entities)
        where TEntity : new()
    {
        var tableProperty = QueryBuilderExtensions.CreateQueryBuilder<TEntity>(false);
        var valProperty = QueryBuilderExtensions.CreateQueryBuilder<TEntity>(true);

        var tableName = typeof(TEntity).Name;
        string sql = $"INSERT INTO {tableName} ({tableProperty}) VALUES ({valProperty})";
        
        foreach (var coupon in entities)
        {
            var affected = await connection.ExecuteAsync(sql, coupon);
        }
    }
}