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
    public string UserId { get; set; } = string.Empty;
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
        foreach (var condition in conditions)
        {
            var entity = await entityService.GetOrCreateByName(condition, token);
            _childConditions.Add(entity);
        }
    }
}
