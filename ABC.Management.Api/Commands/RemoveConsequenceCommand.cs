using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveConsequenceCommand(Consequence Entity)
: IRequest<BaseResponseCommand<Consequence>>
{
    public static RemoveConsequenceCommand Create(Consequence consequence)
    {
        return new(consequence);
    }

    public static RemoveConsequenceCommand Create(Guid consequenceId)
    {
        return new(new Consequence(consequenceId));
    }
}
