using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL.ValidationServices;

public class AntecedentService(IUnitOfWork _uow) : IEntityService<Antecedent>
{
    public async Task<Antecedent?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var antecedents = await _uow.Antecedents
            .GetAsync(a => EF.Functions.ILike(a.Name, name), cancellationToken);
        return antecedents.SingleOrDefault();
    }
}
