using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;

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
        BaseResponseCommand<Consequence> response = new();
        await _uow.Consequences.RemoveAsync(id, cancellationToken);
        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}