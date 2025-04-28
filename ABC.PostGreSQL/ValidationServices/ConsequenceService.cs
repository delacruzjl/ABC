using ABC.Management.Domain.Entities;
using ABC.SharedKernell;

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

    public async Task<Consequence?> GetValue(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await _uow.Consequences.FindAsync(id, cancellationToken);
}
