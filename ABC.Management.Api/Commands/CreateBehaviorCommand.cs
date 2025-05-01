using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateBehaviorCommand(Behavior Value)
    : IRequest<BaseResponseCommand<Behavior>>
{
    public static CreateBehaviorCommand Create(string name, string description)
    {

        Behavior behavior = new()
        {
            Name = name,
            Description = description
        };

        return new(behavior);
    }
}
