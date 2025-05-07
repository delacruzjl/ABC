using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class ChildConditions
{
    [Mutation]
    [GraphQLDescription("Add a new ChildCondition")]
    public static async Task<ChildCondition?> CreateChildCondition(
        IMediator handler,
        string name,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = CreateChildConditionResponseCommand.Create(name);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [GraphQLDescription("Remove a ChildCondition")]
    public static async Task<bool> RemoveChildCondition(
       IMediator handler,
       Guid ChildConditionId,
       IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = RemoveChildConditionResponseCommand.Create(ChildConditionId);
        _ = await command.ExecuteHandler(handler, context, cancellationToken);
        return !context.HasErrors;
    }

    [Query]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve ChildConditions options")]
    public static async Task<IQueryable<ChildCondition>> GetChildConditions(
        IUnitOfWork uow,
        CancellationToken cancellationToken)
         => await uow.ChildConditions.GetAsync(cancellationToken);
}
