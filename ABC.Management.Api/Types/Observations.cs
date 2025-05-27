using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class Observations
{
    [Mutation]
    [GraphQLDescription("Start an observation")]
    public static async Task<Observation?> StartObservation(
        IMediator handler,
        Guid childId,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        StartObservationCommand command = new(childId);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Query]
    [GraphQLDescription("Query observations")]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static async Task<IQueryable<Observation>> GetObservations(
        IUnitOfWork uow,
        CancellationToken cancellationToken)
        => await uow.Observations.GetAsync(cancellationToken);
}
