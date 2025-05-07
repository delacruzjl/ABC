using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveChildConditionResponseCommand(ChildCondition Entity)
: IRequest<BaseResponseCommand<ChildCondition>>
{
    public static RemoveChildConditionResponseCommand Create(Guid childConditionId)
    {
        return new(new ChildCondition(childConditionId));
    }
}
