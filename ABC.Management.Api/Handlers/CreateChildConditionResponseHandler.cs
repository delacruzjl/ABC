using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateChildConditionResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<CreateChildConditionResponseCommand, BaseResponseCommand<ChildCondition>>
{
    public async ValueTask<BaseResponseCommand<ChildCondition>> Handle(
        CreateChildConditionResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<ChildCondition> response = new()
        {
            Entity = await _uow.ChildConditions.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
