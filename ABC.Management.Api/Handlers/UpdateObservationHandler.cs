using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using ABC.SharedKernel.Events;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class UpdateObservationHandler(IUnitOfWork _uow)
    : IRequestHandler<UpdateObservationCommand, BaseResponseCommand<Observation>>
{
    public async ValueTask<BaseResponseCommand<Observation>> Handle(
        UpdateObservationCommand request,
        CancellationToken cancellationToken)
    {
        var observation = await _uow.Observations
            .FindAsync(request.ObservationId, cancellationToken);
        
        var antecedents = await AttachEntities<Antecedent>(
            request.Antecedents,
            observation, 
            cancellationToken);

        var behaviors = await AttachEntities<Behavior>(
            request.Behaviors,
            observation,
            cancellationToken);

        var consequences = await AttachEntities<Consequence>(
            request.Consequences,
            observation,
            cancellationToken);

        observation.Load(
            new NotesUpdated(request.ObservationId, request.Notes ?? string.Empty),
            new AntecedentsUpdated(request.ObservationId, antecedents),
            new BehaviorsUpdated(request.ObservationId, behaviors),
            new ConsequencesUpdated(request.ObservationId, consequences));

        observation = await _uow.Observations.Update(observation, cancellationToken);
        var count = await _uow.SaveChangesAsync();

        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }


        BaseResponseCommand<Observation> response = new(observation);
        return response;
    }

    private async Task<TEntity[]> AttachEntities<TEntity>(
        IEnumerable<Guid>? entityIds, 
        Observation observation,
        CancellationToken cancellationToken) where TEntity : Entity
    {
        
        if (entityIds is null || !entityIds.Any())
        {
            return [];  
        }

        switch (typeof(TEntity).Name)
        {
            case nameof(Antecedent):
                var aTasks = entityIds
                      .Select(async entity =>
                        (TEntity)(object)(await _uow.Antecedents.FindAsync(entity, cancellationToken)))
                      .Where(a => a is not null);
                return await Task.WhenAll(aTasks);
            case nameof(Behavior):
                var bTasks = entityIds
                        .Select(async entity =>
                            (TEntity)(object)(await _uow.Behaviors.FindAsync(entity, cancellationToken)))
                        .Where(a => a is not null);
                return await Task.WhenAll(bTasks);
            case nameof(Consequence):
                var cTask = entityIds
                       .Select(async entity => 
                            (TEntity)(object)(await _uow.Consequences.FindAsync(entity, cancellationToken)))
                       .Where(a => a is not null);
                return await Task.WhenAll(cTask);
        }

        throw new InvalidOperationException($"Unknown entity type: {typeof(TEntity).Name}");
    }
}