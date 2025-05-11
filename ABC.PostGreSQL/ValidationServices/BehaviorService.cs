using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL.ValidationServices;

public class BehaviorService(IUnitOfWork _uow) : IEntityService<Behavior>
{
    public async Task<Behavior?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var behaviors = await _uow.Behaviors
            .GetAsync(a => EF.Functions.ILike(a.Name, name), cancellationToken);
        return behaviors.SingleOrDefault();
    }
}
