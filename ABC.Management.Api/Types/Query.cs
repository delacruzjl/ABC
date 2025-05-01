using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;

namespace ABC.Management.Api.Types;

[QueryType]
public static class Query 
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve antecedents options")]
    public static async Task<IQueryable<Antecedent>> GetAntecedents(IUnitOfWork uow, CancellationToken cancellationToken)
        => await uow.Antecedents.GetAsync(cancellationToken);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve behaviors options")]
    public static async Task<IQueryable<Behavior>> GetBehaviors(IUnitOfWork uow, CancellationToken cancellationToken)
         => await uow.Behaviors.GetAsync(cancellationToken);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve consequences options")]
    public static async Task<IQueryable<Consequence>> GetConsequences(IUnitOfWork uow, CancellationToken ct)
         => await uow.Consequences.GetAsync(ct);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieve available children")]
    public static async Task<IQueryable<Child>> GetChildren(IUnitOfWork uow, CancellationToken ct)
         => await uow.Children.GetAsync(ct);
}
