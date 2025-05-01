using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveChildResponseCommand(Child Entity)
: IRequest<BaseResponseCommand<Child>>
{
    public static RemoveChildResponseCommand Create(Child child)
    {
        return new(child);
    }

    public static RemoveChildResponseCommand Create(Guid childId)
    {
        return new(new Child(childId));
    }
}