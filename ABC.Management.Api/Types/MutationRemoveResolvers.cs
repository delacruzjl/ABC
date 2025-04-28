using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

[ExtendObjectType(nameof(Mutation))]
public sealed class MutationRemoveResolvers
{
    public async Task<bool> RemoveAntecedent(
        IMediator handler,
        Guid antecedentId,
        IResolverContext context)
    {
        var command = RemoveAntecedentCommand.Create(antecedentId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    public async Task<bool> RemoveBehavior(
       IMediator handler,
       Guid behaviorId,
       IResolverContext context)
    {
        var command = RemoveBehaviorCommand.Create(behaviorId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    public async Task<bool> RemoveConsequence(
       IMediator handler,
       Guid consequenceId,
       IResolverContext context)
    {
        var command = RemoveConsequenceCommand.Create(consequenceId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }

    public async Task<bool> RemoveChild(
       IMediator handler,
       Guid childId,
       IResolverContext context)
    {
        var command = RemoveChildCommand.Create(childId);
        _ = await command.ExecuteHandler(handler, context);
        return !context.HasErrors;
    }
}
