using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace Catalog.API.Repositories;

public class Repository<TEntity> : RepositoryBase<TEntity>
    where TEntity : BaseEntity
{
    public Repository(IConfiguration configuration) : base(configuration)
    {
    }
}