using Platform.Database.Entity.SQL;
using Platform.Database.Helpers;

namespace IdentityServer.Domain.Helpers;

public class Repository<TEntity> : RepositoryBase<TEntity, AuthenContext>
    where TEntity : BaseIdEntity, new()
{
    public Repository(AuthenContext context) : base(context)
    {
    }
}