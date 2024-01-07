using Dapper;
using Discount.Domain.Extensions;
using Npgsql;

namespace Discount.Domain.Common.InitializeDB;

public static class ConfigureDB
{
    public static async Task DropTable<TEntity>(NpgsqlConnection connection)
    {
        var tableName = typeof(TEntity).Name.ToLower();
        var existedQuery = $"SELECT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename  = '{tableName}');";

        var isExisted = await connection.QueryFirstOrDefaultAsync<bool>(existedQuery);

        if (isExisted)
        {
            string query = $"Drop Table {tableName}";
            await connection.ExecuteAsync(query);
        }
    }

    public static async Task CreateTable<TEntity>(NpgsqlConnection connection)
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

        await connection.ExecuteAsync(query);
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
            await connection.ExecuteAsync(sql, coupon);
        }
    }
}