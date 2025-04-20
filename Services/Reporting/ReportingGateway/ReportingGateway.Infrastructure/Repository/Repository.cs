using Microsoft.Extensions.Configuration;
using Platform.Database.Entity.MongoDb;
using Platform.Database.MongoDb;

namespace ReportingGateway.Infrastructure.Repository;

public class Repository<TEntity> : RepositoryBase<TEntity>
    where TEntity : BaseEntity
{
    public Repository(IConfiguration configuration) : base(configuration)
    {
    }
}