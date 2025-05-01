using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateConsequenceResponseCommand(Consequence Value)
    : IRequest<BaseResponseCommand<Consequence>>
{
    public static CreateConsequenceResponseCommand Create(string name, string description)
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
