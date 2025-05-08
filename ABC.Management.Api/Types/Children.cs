using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

public class Children
{
    [Mutation]
    [GraphQLDescription("Add a new child")]
    public static async Task<Child?> CreateChild(
        IMediator handler,
        string lastName,
        string firstName,
        int birthYear,
        IEnumerable<string>? conditions,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = CreateChildResponseCommand.Create(lastName, firstName, birthYear, conditions ?? []);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [GraphQLDescription("Remove a child")]
    public static async Task<bool> RemoveChild(
      IMediator handler,
      Guid childId,
      IResolverContext context,
       CancellationToken cancellationToken)
    {
        var command = RemoveChildResponseCommand.Create(childId);
        _ = await command.ExecuteHandler(handler, context, cancellationToken);
        return !context.HasErrors;
    }

    [Query]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve available children")]
    public static async Task<IQueryable<Child>> GetChildren(IUnitOfWork uow, CancellationToken ct)
         => await uow.Children.GetAsync(ct);
}
