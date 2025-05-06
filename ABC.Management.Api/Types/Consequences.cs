using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class Consequences
{
    [Mutation]
    [GraphQLDescription("Add a new consequence")]
    public static async Task<Consequence?> CreateConsequence(
       IMediator handler,
       string name,
       string description,
       IResolverContext context,
       CancellationToken cancellationToken)
    {
        var command = CreateConsequenceResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Query]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve consequences options")]
    public static async Task<IQueryable<Consequence>> GetConsequences(
        IUnitOfWork uow,
        CancellationToken ct)
         => await uow.Consequences.GetAsync(ct);

    [Mutation]
    [GraphQLDescription("Remove a consequence")]
    public static async Task<bool> RemoveConsequence(
       IMediator handler,
       Guid consequenceId,
       IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = RemoveConsequenceResponseCommand.Create(consequenceId);
        _ = await command.ExecuteHandler(handler, context, cancellationToken);
        return !context.HasErrors;
    }
}
