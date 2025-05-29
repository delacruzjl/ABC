using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ABC.Management.Api.Handlers;

public class RemoveConsequenceResponseHandler(
    IUnitOfWork _uow) 
    : IRequestHandler<RemoveConsequenceResponseCommand, BaseResponseCommand<Consequence>>
{
    public async ValueTask<BaseResponseCommand<Consequence>> Handle(
        RemoveConsequenceResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;

        var consequenceQry = await _uow.Consequences
            .GetAsync(a => a.Id == id, cancellationToken);

        var consequence = consequenceQry.Include(a => a.Observations)
            .Single();

        if ((consequence.Observations ?? []).Count != 0)
        {
            throw new InvalidOperationException(
                "Cannot remove consequence that is associated with an observation");
        }

        await _uow.Consequences.RemoveAsync(id, cancellationToken);
        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        BaseResponseCommand<Consequence> response = new();
        return response;
    }
}