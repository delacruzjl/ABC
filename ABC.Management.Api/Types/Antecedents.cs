using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Mediator;
using System.Diagnostics.CodeAnalysis;

namespace ABC.Management.Api.Types;

public class Antecedents
{
    [Mutation]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Add a new antecedent")]
    public static async Task<Antecedent?> CreateAntecedentAsync(
       IMediator handler,
       string name,
       [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string description,
       IResolverContext context,
       CancellationToken cancellationToken)
    {
        var command = CreateAntecedentResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [Authorize(Roles = ["Admin"])]
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
    [Authorize]
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
