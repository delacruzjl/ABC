using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ABC.Management.Api.Handlers;

public class RemoveChildResponseHandler(
    IUnitOfWork _uow) : IRequestHandler<RemoveChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        RemoveChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;

        var childQry = await _uow.Children
           .GetAsync(a => a.Id == id, cancellationToken);

        var child = childQry.Include(a => a.Observations)
            .Single();

        if ((child.Observations ?? []).Count != 0)
        {
            throw new InvalidOperationException(
                "Cannot remove child that is associated with an observation");
        }

        await _uow.Children.RemoveAsync(id, cancellationToken);
        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        BaseResponseCommand<Child> response = new();

        return response;
    }
}
