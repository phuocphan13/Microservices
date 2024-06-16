using Ordering.Infrastruture.Persistence;
using Platform.Database.Helpers;

namespace Ordering.Infrastruture.Database;

public class Repository<TEntity> : RepositoryBase<TEntity, OrderContext>
    where TEntity : class
{
    public Repository(OrderContext context) : base(context)
    {
    }
}