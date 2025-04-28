using ABC.Management.Domain.Entities;
using ABC.SharedKernell;

namespace ABC.PostGreSQL.ValidationServices;

public class BehaviorService(UnitOfWork _uow) : IEntityService<Behavior>
{
    public async Task<Behavior?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var behaviors = await _uow.Behaviors.GetAsync(a => 
            a.Name == name, cancellationToken);
        return behaviors.SingleOrDefault();
    }

    public async Task<Behavior?> GetValue(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await _uow.Behaviors.FindAsync(id, cancellationToken);
}
