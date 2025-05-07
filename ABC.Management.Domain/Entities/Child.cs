namespace ABC.Management.Domain.Entities;
public class Child(
    Guid id,
    string lastName,
    string firstName,
    int birthYear,
    List<ChildCondition> conditions) : Entity(id)
{

    public string LastName { get; init; } = lastName;
    public string FirstName { get; init; } = firstName;
    public int BirthYear { get; init; } = birthYear;
    public ICollection<ChildCondition> Conditions { get; init; } = conditions;

    public Child(Guid id) : this(id, string.Empty, string.Empty, 0, [])
    {

    }

    public Child() : this(Guid.NewGuid())
    {

    }
}
