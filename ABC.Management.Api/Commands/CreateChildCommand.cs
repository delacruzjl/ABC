using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateChildCommand(Child Value)
    : IRequest<BaseResponseCommand<Child>>
{
    internal static CreateChildCommand Create(
        string lastName,
        string firstName,
        int birthYear,
        params IEnumerable<string>? conditions)
    {
        Child child = new()
        {
            LastName = lastName,
            FirstName = firstName,
            BirthYear = birthYear,
            Conditions = conditions?.ToList() ?? []
        };

        CreateChildCommand command = new(child);
        return command;
    }
}