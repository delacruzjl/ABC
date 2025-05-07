namespace ABC.Management.Domain.Entities;

public class ChildCondition(
    Guid id,
    string name,
    List<Child> children) 
    : Entity(id)
{
    public string Name { get; init; } = name;
    public ICollection<Child> children { get; init; } = children;

    public ChildCondition(Guid id, string name) 
        : this(id, name, [])
    {

    }

    public ChildCondition(string name) : this(Guid.NewGuid(), name)
    {
    }

    public ChildCondition(Guid id) : this(id, string.Empty)
    {
    }

    public ChildCondition() : this(string.Empty)
    {
    }

    public static implicit operator string(ChildCondition value) => value.Name;

    public static implicit operator ChildCondition(string value) => new(value);
}
