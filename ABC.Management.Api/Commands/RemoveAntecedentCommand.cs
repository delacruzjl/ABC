using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveAntecedentCommand(Antecedent Entity)
    : IRequest<BaseResponseCommand<Antecedent>>
{
    public static RemoveAntecedentCommand Create(Antecedent antecedent)
    {
        return new(antecedent);
    }

    public static RemoveAntecedentCommand Create(Guid antecedentId)
    {
        return new(new Antecedent(antecedentId));
    }
}
