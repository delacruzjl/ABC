using ABC.Management.Domain.Entities;
using ABC.SharedKernel;

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
}
