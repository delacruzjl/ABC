namespace ABC.SharedKernel;

public interface IEntityService<T> where T : Entity
{
    Task<T?> GetByName(string name, CancellationToken cancellationToken = default);
}
