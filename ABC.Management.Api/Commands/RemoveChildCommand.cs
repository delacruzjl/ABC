using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveChildCommand(Child Entity)
: IRequest<BaseResponseCommand<Child>>
{
    public static RemoveChildCommand Create(Child child)
    {
        return new(child);
    }

    public static RemoveChildCommand Create(Guid childId)
    {
        return new(new Child(childId));
    }
}