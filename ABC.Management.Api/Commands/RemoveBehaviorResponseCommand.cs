using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveBehaviorResponseCommand(Behavior Entity)
: IRequest<BaseResponseCommand<Behavior>>
{
    public static RemoveBehaviorResponseCommand Create(Behavior behavior)
    {
        return new(behavior);
    }

    public static RemoveBehaviorResponseCommand Create(Guid behaviorId)
    {
        return new(new Behavior(behaviorId));
    }
}
