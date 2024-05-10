using Platform.Database.Helpers;

namespace IdentityServer.Domain.Helpers;

public class Repository<TEntity> : RepositoryBase<TEntity, AuthenContext>
    where TEntity : class
{
    public Repository(AuthenContext context) : base(context)
    {
    }
}