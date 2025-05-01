using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveAntecedentResponseCommand(Antecedent Entity)
    : IRequest<BaseResponseCommand<Antecedent>>
{
    public static RemoveAntecedentResponseCommand Create(Antecedent antecedent)
    {
        return new(antecedent);
    }

    public static RemoveAntecedentResponseCommand Create(Guid antecedentId)
    {
        return new(new Antecedent(antecedentId));
    }
}
