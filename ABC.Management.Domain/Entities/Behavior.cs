namespace ABC.Management.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Behavior(
    Guid id,
    string name,
    string description) : Entity(id)
{
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;

    public Behavior(Guid id)
        : this(id, string.Empty, string.Empty)
    {

    }

    public Behavior()
        : this(Guid.NewGuid())
    {

    }
}
