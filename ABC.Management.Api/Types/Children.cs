using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Mediator;
using System.Security.Claims;

namespace ABC.Management.Api.Types;

public class Children
{
    [Mutation]
    [Authorize]
    [GraphQLDescription("Add a new child")]
    public static async Task<Child?> CreateChild(
        IMediator handler,
        string lastName,
        string firstName,
        int birthYear,
        IEnumerable<string>? conditions,
        string? userId,
        ClaimsPrincipal claimsPrincipal,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var isAdmin = claimsPrincipal.IsInRole("Admin");
        var assignedUserId = isAdmin && !string.IsNullOrWhiteSpace(userId)
            ? userId
            : claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = CreateChildResponseCommand.Create(lastName, firstName, birthYear, conditions ?? [], assignedUserId);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Update an existing child (admin only)")]
    public static async Task<Child?> UpdateChild(
        IMediator handler,
        Guid childId,
        string lastName,
        string firstName,
        int birthYear,
        string userId,
        IEnumerable<string>? conditions,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        var command = UpdateChildResponseCommand.Create(childId, lastName, firstName, birthYear, userId, conditions ?? []);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [Authorize]
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
    [Authorize]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve available children")]
    public static async Task<IQueryable<Child>> GetChildren(
        IUnitOfWork uow,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken ct)
    {
        var queryable = await uow.Children.GetAsync(ct);
        var isAdmin = claimsPrincipal.IsInRole("Admin");
        if (isAdmin)
            return queryable;

        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return queryable.Where(c => c.UserId == userId);
    }
}
