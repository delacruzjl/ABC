using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ABC.Management.Api.Handlers;

public class RemoveBehaviorResponseHandler(
    IUnitOfWork _uow) 
    : IRequestHandler<RemoveBehaviorResponseCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        RemoveBehaviorResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;

        var behaviorQry = await _uow.Behaviors
            .GetAsync(a => a.Id == id, cancellationToken);

        var behavior = behaviorQry.Include(a => a.Observations)
            .Single();

        if ((behavior.Observations ?? []).Count != 0)
        {
            throw new InvalidOperationException(
                "Cannot remove behavior that is associated with an observation");
        }

        await _uow.Behaviors.RemoveAsync(id, cancellationToken);
        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        BaseResponseCommand<Behavior> response = new();
        return response;
    }
}
