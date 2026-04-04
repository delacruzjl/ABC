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

    public async Task<ChildCondition> GetOrCreateByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await GetByName(name, cancellationToken);
        if (existing is not null)
            return existing;

        var newCondition = new ChildCondition { Name = name.Trim() };
        await _uow.ChildConditions.AddAsync(newCondition, cancellationToken);
        return newCondition;
    }
}
