using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using ABC.SharedKernel.Events;
using Mediator;
using System.Data;

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
                var antecedents = await Task.WhenAll(entityIds
                    .Select(entity => TryFindEntity(() => _uow.Antecedents.FindAsync(entity, cancellationToken))));
                return antecedents
                    .Where(a => a is not null)
                    .Cast<TEntity>()
                    .ToArray();
            case nameof(Behavior):
                var behaviors = await Task.WhenAll(entityIds
                    .Select(entity => TryFindEntity(() => _uow.Behaviors.FindAsync(entity, cancellationToken))));
                return behaviors
                    .Where(b => b is not null)
                    .Cast<TEntity>()
                    .ToArray();
            case nameof(Consequence):
                var consequences = await Task.WhenAll(entityIds
                    .Select(entity => TryFindEntity(() => _uow.Consequences.FindAsync(entity, cancellationToken))));
                return consequences
                    .Where(c => c is not null)
                    .Cast<TEntity>()
                    .ToArray();
        }

        throw new InvalidOperationException($"Unknown entity type: {typeof(TEntity).Name}");
    }

    private static async Task<TEntityType?> TryFindEntity<TEntityType>(Func<Task<TEntityType>> findEntity)
        where TEntityType : Entity
    {
        try
        {
            return await findEntity();
        }
        catch (DataException)
        {
            return null;
        }
    }
}
