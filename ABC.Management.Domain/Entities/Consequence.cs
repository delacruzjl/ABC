namespace ABC.Management.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Consequence(
    Guid id,
    string name,
    string description,
    List<Observation> observations) : Entity(id)
{
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;
    public IReadOnlyCollection<Observation> Observations =>
        observations.AsReadOnly();

    public Consequence(
    Guid id,
    string name,
    string description)
        : this(id, name, description, [])
    {

    }

    public Consequence(Guid id)
        : this(id, string.Empty, string.Empty)
    {

    }

    public Consequence()
        : this(Guid.NewGuid())
    {

    }
}
