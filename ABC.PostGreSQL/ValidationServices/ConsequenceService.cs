using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL.ValidationServices;

public class ConsequenceService(IUnitOfWork _uow) : IEntityService<Consequence>
{
    public async Task<Consequence?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var consequences = await _uow.Consequences
            .GetAsync(a => EF.Functions.ILike(a.Name, name), cancellationToken);
        return consequences.SingleOrDefault();
    }
}
