using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateChildResponseCommand(Child Value)
    : IRequest<BaseResponseCommand<Child>>
{
    public static CreateChildResponseCommand Create(
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

        CreateChildResponseCommand command = new(child);
        return command;
    }
}