using Platform.Database.Helpers;

namespace IdentityServer.Domain.Helpers;

public class UnitOfWork : UnitOfWorkBase<AuthenContext>
{
    public UnitOfWork(AuthenContext authenContext, ILogger<UnitOfWork> logger) : base(authenContext, logger)
    {
    }
}