using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record UpdateChildResponseCommand(
    Guid ChildId,
    string LastName,
    string FirstName,
    int BirthYear,
    string UserId,
    IEnumerable<string> Conditions) : IRequest<BaseResponseCommand<Child>>
{
    public static UpdateChildResponseCommand Create(
        Guid childId,
        string lastName,
        string firstName,
        int birthYear,
        string userId,
        IEnumerable<string> conditions) =>
        new(childId, lastName, firstName, birthYear, userId, conditions);
}
