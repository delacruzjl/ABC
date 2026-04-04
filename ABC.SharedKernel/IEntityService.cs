namespace ABC.SharedKernel;

public interface IEntityService<T> where T : Entity
{
    Task<T?> GetByName(string name, CancellationToken cancellationToken = default);
    Task<T> GetOrCreateByName(string name, CancellationToken cancellationToken = default)
        => throw new NotSupportedException();
}
