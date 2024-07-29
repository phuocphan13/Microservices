using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Platform.Database.Entity.SQL;

namespace Platform.Database.Helpers;

public interface IRepository<TEntity> where TEntity : BaseIdEntity
{
    IQueryable<TEntity> GetAll();

    List<TEntity> GetAllList();

    Task<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken = default);

    List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

    TEntity Single(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    void Insert(TEntity entity);

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Insert(IEnumerable<TEntity> entities);

    Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Update(IEnumerable<TEntity> entities);
        
    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);
}

public class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
    where TEntity : BaseIdEntity, new()
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly DbSet<TEntity> Entities;

    protected RepositoryBase(TContext context)
    {
        Context = context;
        Entities = context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return Entities.AsNoTracking();
    }

    public virtual List<TEntity> GetAllList()
    {
        return GetAll().ToList();
    }

    public virtual async Task<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken)
    {
        return await GetAll().ToListAsync(cancellationToken);
    }

    public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
    {
        return GetAll().Where(predicate).ToList();
    }

    public virtual async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await GetAll().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
    {
        return GetAll().Where(predicate);
    }

    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.FirstOrDefault(predicate);
    }

    public virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return Entities.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return Entities.AnyAsync(predicate, cancellationToken);
    }

    public virtual void Insert(TEntity entity)
    {
        Entities.Add(entity);
    }

    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        AddAuditInfo(entity);
        await Entities.AddAsync(entity, cancellationToken);
    }

    public virtual void Insert(IEnumerable<TEntity> entities)
    {
        AddAuditInfoList(entities);
        Entities.AddRange(entities);
    }

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        AddAuditInfoList(entities);
        await Entities.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        AddAuditInfo(entity, true);
        Entities.Update(entity);
    }

    public virtual void Update(IEnumerable<TEntity> entities)
    {
        AddAuditInfoList(entities, true);
        Entities.UpdateRange(entities);
    }

    public TEntity Single(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Single(predicate);
    }

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Entities.SingleAsync(predicate);
    }

    public void Delete(TEntity entity)
    {
        Entities.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        Entities.RemoveRange(entities);
    }
    
    private void AddAuditInfoList(IEnumerable<TEntity> entities, bool isUpdate = false)
    {
        foreach (var entity in entities)
        {
            AddAuditInfo(entity, isUpdate);
        }
    }

    private void AddAuditInfo(TEntity entity, bool isUpdate = false)
    {
        if (entity is EntityBase model)
        {
            if (isUpdate)
            {
                model.LastModifiedDate = DateTime.UtcNow;
                model.LastModifiedBy = "Lucifer";
            }
            else
            {
                model.CreatedBy = "Lucifer";
                model.CreatedDate = DateTime.UtcNow;
            }
        }
    }
}