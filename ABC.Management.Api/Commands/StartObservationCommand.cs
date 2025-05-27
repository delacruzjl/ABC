using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record StartObservationCommand(Guid ChildId)
    : IRequest<BaseResponseCommand<Observation>>
{
    
}
