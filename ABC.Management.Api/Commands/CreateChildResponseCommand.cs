using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateChildResponseCommand(Child Value, IEnumerable<string> Conditions)
    : IRequest<BaseResponseCommand<Child>>
{
    public static CreateChildResponseCommand Create(
        string lastName,
        string firstName,
        int birthYear,
        IEnumerable<string> conditions)
    {
        Child child = new()
        {
            LastName = lastName,
            FirstName = firstName,
            BirthYear = birthYear
        };

        CreateChildResponseCommand command = new(child, conditions);
        return command;
    }
}