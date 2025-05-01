using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record RemoveConsequenceResponseCommand(Consequence Entity)
: IRequest<BaseResponseCommand<Consequence>>
{
    public static RemoveConsequenceResponseCommand Create(Consequence consequence)
    {
        return new(consequence);
    }

    public static RemoveConsequenceResponseCommand Create(Guid consequenceId)
    {
        return new(new Consequence(consequenceId));
    }
}
