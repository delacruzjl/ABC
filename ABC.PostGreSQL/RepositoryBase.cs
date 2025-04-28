using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace ABC.PostGreSQL;
public class RepositoryBase<TContext,TEntity> : IRepository<TEntity> 
    where TEntity : Entity
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    private bool _disposed = false;
    protected DbSet<TEntity> _dbSet { get; set; }

    public RepositoryBase(TContext dbContext)
    {
        _dbContext = dbContext ??
            throw new ArgumentNullException(nameof(dbContext));

        _dbSet = _dbContext.Set<TEntity>();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _dbContext.Dispose();
        }

        _disposed = true;
    }

    public async Task<TEntity> FindAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(id, cancellationToken) 
            ?? throw new DataException($"Entity with Id: {id} not found.");
        
        _dbContext.Entry(entity).State = EntityState.Unchanged;

        return entity;
    }

    public Task<IQueryable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(_dbSet.Where(predicate));

    public Task<IQueryable<TEntity>> GetAsync(
        CancellationToken cancellationToken = default) =>
        Task.FromResult(_dbSet.AsQueryable());

    public Task<TEntity> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        var newEntry = _dbContext.Entry(entity);
        if (newEntry.State != EntityState.Detached)
        {
            newEntry.State = EntityState.Added;
            return Task.FromResult(newEntry.Entity);
        }

        _dbSet.Add(entity);
        return Task.FromResult(entity);
    }

    public async Task RemoveAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken);
        var dbEntityEntry = _dbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Deleted)
        {
            dbEntityEntry.State = EntityState.Deleted;
            return;
        }

        _dbSet.Attach(entity);
        _dbSet.Remove(entity);        
    }
}