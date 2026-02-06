using ABC.Management.Domain.Entities;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Commands;

public record StartObservationCommand(Guid ChildId)
    : IRequest<BaseResponseCommand<Observation>>
{

}

public record EndObservationCommand(Guid ObservationId)
     : IRequest<BaseResponseCommand<Observation>>
{

}

public record class UpdateObservationCommand(
    Guid ObservationId,
    List<Guid>? Antecedents,
    List<Guid>? Behaviors,
    List<Guid>? Consequences,
    [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? Notes)
    : IRequest<BaseResponseCommand<Observation>>
{
}