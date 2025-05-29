using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel.Events;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class StartObservationHandler(IUnitOfWork _uow)
    : IRequestHandler<StartObservationCommand, BaseResponseCommand<Observation>>
{
    public async ValueTask<BaseResponseCommand<Observation>> Handle(
        StartObservationCommand request,
        CancellationToken cancellationToken)
    {
        var child = await _uow.Children.FindAsync(
            request.ChildId,
            cancellationToken) ?? throw new ValidationException(
                 "Invalid Child Identifier",
                 [
                     new ValidationFailure(
                        nameof(Child),
                        "Child not found")
                 ]);

        Observation observation = new(
            Guid.NewGuid(),
            child,
            string.Empty);

        observation.Load(
            new ObservationStarted(observation.Id, child?.Id, DateTime.UtcNow));

        await _uow.Observations.AddAsync(observation, cancellationToken);
        
        BaseResponseCommand<Observation> response = new(observation);
        var count = await _uow.SaveChangesAsync();
        
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
