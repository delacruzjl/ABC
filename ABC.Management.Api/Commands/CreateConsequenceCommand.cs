using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateConsequenceCommand(Consequence Value)
    : IRequest<BaseResponseCommand<Consequence>>
{
    public static CreateConsequenceCommand Create(string name, string description)
    {
        Consequence consequence = new()
        {
            Name = name,
            Description = description
        };

        CreateConsequenceCommand command = new(consequence);
        return command;
    }
}
