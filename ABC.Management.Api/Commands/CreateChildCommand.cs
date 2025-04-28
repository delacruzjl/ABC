using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateChildCommand(Child Value)
    : IRequest<BaseResponseCommand<Child>>
{
    internal static CreateChildCommand Create(string lastName, string firstName, int age)
    {
        Child child = new()
        {
            LastName = lastName,
            FirstName = firstName,
            Age = age
        };

        CreateChildCommand command = new(child);
        return command;
    }
}