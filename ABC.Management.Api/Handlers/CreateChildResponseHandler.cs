using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;
using System.Collections;

namespace ABC.Management.Api.Handlers;

public class CreateChildResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<CreateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        var childConditions = await FetchChildConditions(request, cancellationToken);
        EnsureChildConditionsExist(childConditions);
        PopulateEntityConditions(request, childConditions);

        BaseResponseCommand<Child> response = new()
        {
            Entity = await _uow.Children.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }

    private static void PopulateEntityConditions(CreateChildResponseCommand request, SortedList childConditions)
    {
        request.Value.Conditions.Clear();
        foreach (var key in childConditions.Keys)
        {
            var childCondition = childConditions[key] as ChildCondition;
            if (childCondition != null)
            {
                request.Value.Conditions.Add(childCondition);
            }
        }
    }

    private static void EnsureChildConditionsExist(SortedList childConditions)
    {
        if (childConditions.ContainsValue(null))
        {
            var notFound = childConditions
                .OfType<DictionaryEntry>()
                .Where(c => c.Value == null)
                .Select(c => c.Key)
                .ToList();

            throw new InvalidOperationException(
                $"Child conditions not found: {string.Join(", ", notFound)}");
        }
    }

    private async Task<SortedList> FetchChildConditions(CreateChildResponseCommand request, CancellationToken cancellationToken)
    {
        SortedList childConditions = new();
        foreach (var condition in request.Conditions)
        {
            var childCondition = await _uow.ChildConditions.GetAsync(c => c.Name == condition, cancellationToken);
            childConditions[condition] = childCondition.SingleOrDefault();
        }

        return childConditions;
    }
}
