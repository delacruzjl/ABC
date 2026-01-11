using ABC.Management.Domain.Entities;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Commands;

public record CreateBehaviorResponseCommand(Behavior Value)
    : IRequest<BaseResponseCommand<Behavior>>
{
    public static CreateBehaviorResponseCommand Create(
        string name,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string description)
    {

        Behavior behavior = new()
        {
            Name = name,
            Description = description
        };

        return new(behavior);
    }
}
