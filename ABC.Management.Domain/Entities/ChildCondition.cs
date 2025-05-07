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

    public ChildCondition() : this(string.Empty)
    {
    }
}
