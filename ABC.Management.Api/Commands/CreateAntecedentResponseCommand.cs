using ABC.Management.Domain.Entities;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Commands;

public record CreateAntecedentResponseCommand(Antecedent Value)
    : IRequest<BaseResponseCommand<Antecedent>>
{

    public static CreateAntecedentResponseCommand Create(
        string name,
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string description)
    {
        Antecedent antecedent = new()
        {
            Name = name,
            Description = description
        };

        return new(antecedent);
    }

}
