using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ABC.Management.Api.Handlers;

public class RemoveAntecedentHandler(
    IUnitOfWork _uow) : IRequestHandler<RemoveAntecedentResponseCommand, BaseResponseCommand<Antecedent>>
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        RemoveAntecedentResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;

        var antecedentQry = await _uow.Antecedents
            .GetAsync(a => a.Id == id, cancellationToken);

        var antecedent = antecedentQry.Include(a => a.Observations)
            .Single();

        if ((antecedent.Observations ?? []).Count != 0)
        {
            throw new InvalidOperationException(
                "Cannot remove antecedent that is associated with an observation");
        }

        await _uow.Antecedents.RemoveAsync(id, cancellationToken);        
        var count = await _uow.SaveChangesAsync();

        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return new BaseResponseCommand<Antecedent>();
    }
}
