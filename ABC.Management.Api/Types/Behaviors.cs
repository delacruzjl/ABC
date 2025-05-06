using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class Behaviors
{
    [Mutation]
    [GraphQLDescription("Add a new behavior")]
    public static async Task<Behavior?> CreateBehavior(
        IMediator handler,
        string name,
        string description,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = CreateBehaviorResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [GraphQLDescription("Remove a behavior")]
    public static async Task<bool> RemoveBehavior(
       IMediator handler,
       Guid behaviorId,
       IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = RemoveBehaviorResponseCommand.Create(behaviorId);
        _ = await command.ExecuteHandler(handler, context, cancellationToken);
        return !context.HasErrors;
    }

    [Query]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve behaviors options")]
    public static async Task<IQueryable<Behavior>> GetBehaviors(
        IUnitOfWork uow,
        CancellationToken cancellationToken)
         => await uow.Behaviors.GetAsync(cancellationToken);
}
