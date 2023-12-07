using System.Linq.Expressions;
using Catalog.API.Common;
using Catalog.API.Common.Consts;
using Catalog.API.Common.Helpers;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<List<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken = default);
    Task<TEntity> GetEntityFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetEntitiesQueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task CreateEntityAsync(TEntity product, CancellationToken cancellationToken = default);
    Task<bool> UpdateEntityAsync(TEntity product, CancellationToken cancellationToken = default);
    Task<bool> DeleteEntityAsync(string id, CancellationToken cancellationToken = default);
}

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity: BaseEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public Repository(IConfiguration configuration)
    {
        var database = new MongoClient(configuration.GetValue<string>(DatabaseConst.CollectionName.ConnectionString)).GetDatabase(configuration.GetValue<string>(DatabaseConst.CollectionName.DatabaseName));
        _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
    }

    private protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .First()).CollectionName;
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

    public async Task CreateEntityAsync(TEntity product, CancellationToken cancellationToken)
    {
        var options = new InsertOneOptions()
        {
            BypassDocumentValidation = false
        };

        product.Id = ModelHelpers.GenerateId();

        await _collection.InsertOneAsync(product, options, cancellationToken);
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