using ABC.Management.Domain.Entities;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Commands;

public record StartObservationCommand(Guid ChildId, DailyContextInput? DailyContext = null)
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
    [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? Notes,
    DailyContextInput? DailyContext = null)
    : IRequest<BaseResponseCommand<Observation>>
{
}

public record DailyContextInput(
    bool HadBreakfast = false,
    bool HadLunch = false,
    bool HadDinner = false,
    bool HadSnack = false,
    bool SleptWell = false,
    int? HoursOfSleep = null)
{
    public ABC.SharedKernel.DailyContext ToDomain() => new(
        HadBreakfast, HadLunch, HadDinner, HadSnack, SleptWell, HoursOfSleep);
}