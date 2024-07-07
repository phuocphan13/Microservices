using Ordering.Infrastructure.Persistence;
using Platform.Database.Entity;
using Platform.Database.Helpers;

namespace Ordering.Infrastructure.Database;

public class Repository<TEntity> : RepositoryBase<TEntity, OrderContext>
    where TEntity : EntityBase, new()
{
    public Repository(OrderContext context) : base(context)
    {
    }
}