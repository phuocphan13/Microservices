using Dapper;
using Discount.Domain.Entities;
using Discount.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Domain.Repositories.Common;

public interface IBaseRepository
{
    Task<bool> AnyAsync<TEntity>(string query, object param) where TEntity : ExtendEntity, new();
    Task<TEntity?> QueryFirstOrDefaultAsync<TEntity>(string query, object param) where TEntity : ExtendEntity, new();
    Task<TEntity> CreateEntityAsync<TEntity>(TEntity entity) where TEntity : ExtendEntity, new();
    Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : ExtendEntity, new();
    Task<bool> DeleteEntityAsync<TEntity>(int id) where TEntity : ExtendEntity, new();
}

public class BaseRepository : IBaseRepository
{
    private readonly IConfiguration _configuration;

    public BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private NpgsqlConnection InitializaCollection()
    {
        var connection = new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);

        return connection;
    }

    public async Task<bool> AnyAsync<TEntity>(string query, object param)
        where TEntity : ExtendEntity, new()
    {
        using var connection = InitializaCollection();
        var tableName = typeof(TEntity).Name;
        string sql = $"SELECT count(Id) FROM {tableName} WHERE " + query;

        var count = await connection.ExecuteAsync(sql, param);
        return count > 0;
    }

    public async Task<TEntity?> QueryFirstOrDefaultAsync<TEntity>(string query, object param)
        where TEntity: ExtendEntity, new()
    {
        using var connection = InitializaCollection();
        var tableName = typeof(TEntity).Name;
        string sql = $"SELECT * FROM {tableName} WHERE " + query;

        var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(sql, param);

        return entity;
    }

    public async Task<TEntity> CreateEntityAsync<TEntity>(TEntity entity)
        where TEntity : ExtendEntity, new()
    {
        using var connection = InitializaCollection();
        var tableProperty = QueryBuilderExtensions.CreateQueryBuilder<TEntity>(false);
        var valProperty = QueryBuilderExtensions.CreateQueryBuilder<TEntity>(true);

        string sql = $"INSERT INTO {nameof(entity)} ({tableProperty}) VALUES ({valProperty})";

        entity.CreatedBy = "Admin Created";
        entity.CreatedDate = DateTime.Now;
        entity.IsActive = true;
        
        await connection.ExecuteAsync(sql, entity);

        return entity;
    }

    public async Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity)
        where TEntity : ExtendEntity, new()
    {
        using var connection = InitializaCollection();
        var tableProperty = QueryBuilderExtensions.UpdateQueryBuilder<TEntity>();

        string sql = $"UPDATE {nameof(entity)} SET ({tableProperty}) WHERE Id = @Id";

        entity.UpdatedBy = "Admin Created";
        entity.UpdatedDate = DateTime.Now;

        await connection.ExecuteAsync(sql, entity);

        return entity;
    }

    public async Task<bool> DeleteEntityAsync<TEntity>(int id)
        where TEntity : ExtendEntity, new()
    {
        using var connection = InitializaCollection();

        var tableName = typeof(TEntity).Name;
        string sql = $"DELETE FROM {tableName} WHERE id = @id";

        var affected = await connection.ExecuteAsync(sql, new { Id = id });

        return affected > 0;
    }
}