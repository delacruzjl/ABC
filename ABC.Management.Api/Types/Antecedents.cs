using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class Antecedents
{
    [Mutation]
    [GraphQLDescription("Add a new antecedent")]
    public static async Task<Antecedent?> CreateAntecedentAsync(
       IMediator handler,
       string name,
       string description,
       IResolverContext context,
       CancellationToken cancellationToken)
    {
        var command = CreateAntecedentResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [GraphQLDescription("Remove an antecedent")]
    public static async Task<bool> RemoveAntecedent(
        IMediator handler,
        Guid antecedentId,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = RemoveAntecedentResponseCommand.Create(antecedentId);
        _ = await command.ExecuteHandler(handler, context, cancellationToken);
        return !context.HasErrors;
    }

    [Query]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve antecedents options")]
    public static async Task<IQueryable<Antecedent>> GetAntecedents(
        IUnitOfWork uow,
        CancellationToken cancellationToken)
        => await uow.Antecedents.GetAsync(cancellationToken);
}
