using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

[ExtendObjectType(nameof(Mutation))]
public sealed class MutationRemoveResolvers
{
    [GraphQLDescription("Remove an antecedent")]
    public async Task<bool> RemoveAntecedent(
        IMediator handler,
        Guid antecedentId,
        IResolverContext context)
    {
        var command = RemoveAntecedentResponseCommand.Create(antecedentId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    [GraphQLDescription("Remove a behavior")]
    public async Task<bool> RemoveBehavior(
       IMediator handler,
       Guid behaviorId,
       IResolverContext context)
    {
        var command = RemoveBehaviorResponseCommand.Create(behaviorId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    [GraphQLDescription("Remove a consequence")]
    public async Task<bool> RemoveConsequence(
       IMediator handler,
       Guid consequenceId,
       IResolverContext context)
    {
        var command = RemoveConsequenceResponseCommand.Create(consequenceId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    [GraphQLDescription("Remove a child")]
    public async Task<bool> RemoveChild(
       IMediator handler,
       Guid childId,
       IResolverContext context)
    {
        var command = RemoveChildResponseCommand.Create(childId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }
}
