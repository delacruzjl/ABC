using System.Linq.Expressions;

namespace ABC.SharedKernell;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
