using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL.ValidationServices;

public class ChildConditionService(IUnitOfWork _uow) : IEntityService<ChildCondition>
{
    public async Task<ChildCondition?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var condition = await _uow.ChildConditions
            .GetAsync(a => EF.Functions.ILike(a.Name, name), cancellationToken);
        return condition.SingleOrDefault();
    }
}
