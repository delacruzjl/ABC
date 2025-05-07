using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class RemoveChildConditionResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<RemoveChildConditionResponseCommand, BaseResponseCommand<ChildCondition>>
{
    public async ValueTask<BaseResponseCommand<ChildCondition>> Handle(
        RemoveChildConditionResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;
        BaseResponseCommand<ChildCondition> response = new();

        await _uow.Antecedents.RemoveAsync(id, cancellationToken);
        var count = await _uow.SaveChangesAsync();

        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}