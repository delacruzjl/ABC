using ABC.Management.Domain.Entities;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Commands;

public record CreateConsequenceResponseCommand(Consequence Value)
    : IRequest<BaseResponseCommand<Consequence>>
{
    public static CreateConsequenceResponseCommand Create(
        string name,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string description)
    {
        Consequence consequence = new()
        {
            Name = name,
            Description = description
        };

        CreateConsequenceResponseCommand command = new(consequence);
        return command;
    }
}
