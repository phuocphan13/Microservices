using Platform.Database.Entity;
using Platform.Database.Helpers;

namespace IdentityServer.Domain.Helpers;

public class Repository<TEntity> : RepositoryBase<TEntity, AuthenContext>
    where TEntity : EntityBase, new()
{
    public Repository(AuthenContext context) : base(context)
    {
    }
}