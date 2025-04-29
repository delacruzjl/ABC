using ABC.Management.Domain.Entities;
using ABC.SharedKernel;

namespace ABC.PostGreSQL.ValidationServices;

public class ConsequenceService(UnitOfWork _uow) : IEntityService<Consequence>
{
    public async Task<Consequence?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var consequences = await _uow.Consequences.GetAsync(a => a.Name == name, cancellationToken);
        return consequences.SingleOrDefault();
    }
}
