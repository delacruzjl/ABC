namespace ABC.Management.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Consequence(
    Guid id,
    string name,
    string description) : Entity(id)
{
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;

    public Consequence(Guid id)
        : this(id, string.Empty, string.Empty)
    {

    }

    public Consequence()
        : this(Guid.NewGuid())
    {

    }
}
