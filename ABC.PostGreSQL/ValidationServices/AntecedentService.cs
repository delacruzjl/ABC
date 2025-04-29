using ABC.Management.Domain.Entities;
using ABC.SharedKernel;    

namespace ABC.PostGreSQL.ValidationServices;

public class AntecedentService(IUnitOfWork _uow) : IEntityService<Antecedent>
{
    public async Task<Antecedent?> GetByName(
        string name,
        CancellationToken cancellationToken = default)
    {
        var antecedents = await _uow.Antecedents.GetAsync(a => a.Name == name, cancellationToken);
        return antecedents.SingleOrDefault();
    }
}
