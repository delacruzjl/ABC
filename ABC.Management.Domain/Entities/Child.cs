using FluentValidation.Results;
using System.Collections;

namespace ABC.Management.Domain.Entities;
public class Child(
    Guid id,
    string lastName,
    string firstName,
    int birthYear,
    List<ChildCondition> childConditions,
    List<Observation> _observations) : Entity(id)
{
    private readonly List<ChildCondition> _childConditions = childConditions;

    public string LastName { get; init; } = lastName;
    public string FirstName { get; init; } = firstName;
    public int BirthYear { get; init; } = birthYear;
    public ICollection<ChildCondition> Conditions { get; set; } = childConditions;
    public ICollection<Observation> Observations { get; set; } = _observations;

    public Child(
    Guid id,
    string lastName,
    string firstName,
    int birthYear,
    params List<ChildCondition> childConditions) 
        : this(
              id,
              lastName,
              firstName,
              birthYear,
              childConditions,
              [])
    { }

    public Child(Guid id) 
        : this(
              id,
              string.Empty,
              string.Empty,
              0,
              [])
    {

    }

    public Child() : this(Guid.NewGuid())
    {

    }

    public async Task SetChildConditions(
        IEntityService<ChildCondition> entityService,
        IEnumerable<string> conditions,
        CancellationToken token)
    {
        SortedList results = new();
        foreach (var condition in conditions)
        {
            var exists = await entityService.GetByName(condition, token);
            results[condition] = exists;
        }

        if (!results.ContainsValue(null)) 
        {
            _childConditions.AddRange(results.Values.OfType<ChildCondition>());
            return;
        }

        List<ValidationFailure> failures = new();
        foreach (var key in results.Keys)
        {
            if (results[key] is null)
            {
                var failure = new ValidationFailure(nameof(ChildCondition), $"Child condition not found:{key}", key);
                failures.Add(failure);
            }
        }

        throw new ValidationException("One or more child conditions do not exist in the database", failures);
    }
}
