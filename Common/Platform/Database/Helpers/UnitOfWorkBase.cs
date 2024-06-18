using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Platform.Database.Helpers;

public interface IUnitOfWork
{
    bool SaveChanges();
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public abstract class UnitOfWorkBase<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _gmcContext;
    private readonly ILogger _logger;

    protected UnitOfWorkBase(TContext gmcContext, ILogger<UnitOfWorkBase<TContext>> logger)
    {
        _gmcContext = gmcContext;
        _logger = logger;
    }

    public virtual bool SaveChanges()
    {
        try
        {
            return _gmcContext.SaveChanges() > 0;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError($"Some errors happen while committing - {e.Message}");
            throw;
            //throw new AppException(e.Message, e);
        }
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _gmcContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (DbUpdateConcurrencyException e)
        {
            _logger.LogError($"Some errors happen while committing - {e.Message}");
            throw;
            //throw new AppException(e.Message, e);
        }
    }
}