using Ordering.Infrastructure.Persistence;
using Platform.Database.Entity.SQL;
using Platform.Database.Helpers;

namespace Ordering.Infrastructure.Database;

public class Repository<TEntity> : RepositoryBase<TEntity, OrderContext>
    where TEntity : BaseIdEntity, new()
{
    public Repository(OrderContext context) : base(context)
    {
    }
}