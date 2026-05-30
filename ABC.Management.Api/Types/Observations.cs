using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ABC.Management.Api.Types;

public class Observations
{
    [Mutation]
    [Authorize]
    [GraphQLDescription("Start an observation")]
    public static async Task<Observation?> StartObservation(
        IMediator handler,
        IUnitOfWork uow,
        Guid childId,
        DailyContextInput? dailyContext,
        ClaimsPrincipal claimsPrincipal,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        if (!await IsChildOwnerOrAdmin(uow, childId, claimsPrincipal, cancellationToken))
        {
            context.ReportError(
                ErrorBuilder.New()
                    .SetMessage("You can only start observations for your own children.")
                    .SetCode("AUTH_NOT_OWNER")
                    .Build());
            return null;
        }

        StartObservationCommand command = new(childId, dailyContext);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [Authorize]
    [GraphQLDescription("End an observation")]
    public static async Task<Observation?> EndObservation(
        IMediator handler,
        IUnitOfWork uow,
        Guid observationId,
        ClaimsPrincipal claimsPrincipal,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        if (!await IsObservationOwnerOrAdmin(uow, observationId, claimsPrincipal, cancellationToken))
        {
            context.ReportError(
                ErrorBuilder.New()
                    .SetMessage("You can only end observations for your own children.")
                    .SetCode("AUTH_NOT_OWNER")
                    .Build());
            return null;
        }

        EndObservationCommand command = new(observationId);
        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Mutation]
    [Authorize]
    [GraphQLDescription("Update an observation")]
    public static async Task<Observation?> UpdateObservation(
        IMediator handler,
        IUnitOfWork uow,
        UpdateObservationCommand command,
        ClaimsPrincipal claimsPrincipal,
        IResolverContext context,
        CancellationToken cancellationToken)
    {
        if (!await IsObservationOwnerOrAdmin(uow, command.ObservationId, claimsPrincipal, cancellationToken))
        {
            context.ReportError(
                ErrorBuilder.New()
                    .SetMessage("You can only update observations for your own children.")
                    .SetCode("AUTH_NOT_OWNER")
                    .Build());
            return null;
        }

        return await command.ExecuteHandler(handler, context, cancellationToken);
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Query observations")]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static async Task<IQueryable<Observation>> GetObservations(
        IUnitOfWork uow,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        var queryable = await uow.Observations.GetAsync(cancellationToken);
        var isAdmin = claimsPrincipal.IsInRole("Admin");
        if (isAdmin)
            return queryable;

        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return queryable.Where(o => o.Child != null && o.Child.UserId == userId);
    }

    private static async Task<bool> IsChildOwnerOrAdmin(
        IUnitOfWork uow, Guid childId, ClaimsPrincipal claimsPrincipal, CancellationToken ct)
    {
        if (claimsPrincipal.IsInRole("Admin")) return true;

        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var children = await uow.Children.GetAsync(c => c.Id == childId, ct);
        var child = await children.SingleOrDefaultAsync(ct);
        return child?.UserId == userId;
    }

    private static async Task<bool> IsObservationOwnerOrAdmin(
        IUnitOfWork uow, Guid observationId, ClaimsPrincipal claimsPrincipal, CancellationToken ct)
    {
        if (claimsPrincipal.IsInRole("Admin")) return true;

        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var observations = await uow.Observations.GetAsync(
            o => o.Id == observationId, ct);
        var observation = await observations
            .Include(o => o.Child)
            .SingleOrDefaultAsync(ct);
        return observation?.Child?.UserId == userId;
    }
}
