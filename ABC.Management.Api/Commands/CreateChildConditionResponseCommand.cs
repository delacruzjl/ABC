using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateChildConditionResponseCommand(ChildCondition Value)
    : IRequest<BaseResponseCommand<ChildCondition>>
{
    public static CreateChildConditionResponseCommand Create(string name)
    {
        ChildCondition childCondition = name;
        return new(childCondition);
    }
}
