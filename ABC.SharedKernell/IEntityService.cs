namespace ABC.SharedKernell;

public interface IEntityService<T> where T : Entity
{
    Task<T?> GetValue(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByName(string name, CancellationToken cancellationToken = default);
}
