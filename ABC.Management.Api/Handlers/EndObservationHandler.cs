using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel.Events;
using FluentValidation;
using FluentValidation.Results;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace ABC.Management.Api.Handlers;

public class EndObservationHandler(IUnitOfWork _uow)
    : IRequestHandler<EndObservationCommand, BaseResponseCommand<Observation>>
{
    public async ValueTask<BaseResponseCommand<Observation>> Handle(
        EndObservationCommand request,
        CancellationToken cancellationToken)
    {
        var observationQuery = await _uow.Observations
            .GetAsync(o => o.Id == request.ObservationId, cancellationToken);

        var observation = observationQuery
            .Include(o => o.Antecedents)
            .Include(o => o.Behaviors)
            .Include(o => o.Consequences)
            .FirstOrDefault() ?? throw new ValidationException(
                "Invalid Observation Identifier",
                [
                    new ValidationFailure(
                        nameof(Observation),
                        "Observation not found")
                ]);

        observation.Load(
            new ObservationEnded(observation.Id, DateTime.UtcNow));

        
        observation = await _uow.Observations.Update(observation, cancellationToken);
        var count = await _uow.SaveChangesAsync();

        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        BaseResponseCommand<Observation> response = new(observation);
        return response;
    }
}
