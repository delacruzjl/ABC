using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveBehaviorCommand(Behavior Entity)
: IRequest<BaseResponseCommand<Behavior>>
{
    public static RemoveBehaviorCommand Create(Behavior behavior)
    {
        return new(behavior);
    }

    public static RemoveBehaviorCommand Create(Guid behaviorId)
    {
        return new(new Behavior(behaviorId));
    }
}
