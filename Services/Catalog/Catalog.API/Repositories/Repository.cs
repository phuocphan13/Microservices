using System.Linq.Expressions;
using Catalog.API.Common.Extensions;
using Catalog.API.Entities;
using MongoDB.Driver;
using Platform.Constants;

namespace Catalog.API.Repositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<List<string>> GetEntityIdsAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken = default);
    Task<TEntity> GetEntityFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetEntitiesQueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<TEntity> CreateEntityAsync(TEntity product, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> CreateEntitiesAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<bool> UpdateEntityAsync(TEntity product, CancellationToken cancellationToken = default);
    Task<bool> DeleteEntityAsync(string id, CancellationToken cancellationToken = default);
}

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity: BaseEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public Repository(IConfiguration configuration)
    {
        var database = new MongoClient(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.ConnectionString)).GetDatabase(configuration.GetValue<string>(DatabaseConst.ConnectionSetting.MongoDB.DatabaseName));
        _collection = database.GetCollection<TEntity>(DatabaseExtensions.GetCollectionName(typeof(TEntity)));
    }

    public async Task<List<string>> GetEntityIdsAsync(CancellationToken cancellationToken)
    {
        var entities = await _collection.Find(x => true).ToListAsync(cancellationToken);

        return entities.Select(x => x.Id).ToList();
    }

    public async Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken)
    {
        var entities = await _collection.Find(x => true).ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<TEntity> GetEntityFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var entity = await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);

        return entity;
    }

    public async Task<List<TEntity>> GetEntitiesQueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var entities = await _collection.Find(predicate).ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var isExisted = await _collection.Find(predicate).AnyAsync(cancellationToken);

        return isExisted;
    }

    public async Task<TEntity> CreateEntityAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var options = new InsertOneOptions()
        {
            BypassDocumentValidation = false
        };

        await _collection.InsertOneAsync(entity, options, cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<TEntity>> CreateEntitiesAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var options = new InsertManyOptions()
        {
            BypassDocumentValidation = false
        };

        await _collection.InsertManyAsync(entities, options, cancellationToken);

        return entities;
    }

    public async Task<bool> UpdateEntityAsync(TEntity product, CancellationToken cancellationToken)
    {
        var options = new ReplaceOptions()
        {
            BypassDocumentValidation = false,
        };

        var updateResult = await _collection.ReplaceOneAsync(g => g.Id == product.Id, product, options, cancellationToken);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteEntityAsync(string id, CancellationToken cancellationToken)
    {
        FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        DeleteResult deleteResult = await _collection.DeleteOneAsync(filter, cancellationToken);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}