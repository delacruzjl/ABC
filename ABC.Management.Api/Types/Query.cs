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
    public static async Task<IQueryable<Antecedent>> GetAntecedents(UnitOfWork uow, CancellationToken cancellationToken)
        => await uow.Antecedents.GetAsync(cancellationToken);

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static async Task<IQueryable<Behavior>> GetBehaviors(UnitOfWork uow)
         => await uow.Behaviors.GetAsync();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static async Task<IQueryable<Consequence>> GetConsequences(UnitOfWork uow)
         => await uow.Consequences.GetAsync();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public static async Task<IQueryable<Child>> GetChildren(UnitOfWork uow)
         => await uow.Children.GetAsync();
}
