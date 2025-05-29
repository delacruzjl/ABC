namespace ABC.Management.Domain.Entities;

public class Antecedent(
    Guid id,
    string name,
    string description,
    List<Observation> observations) : Entity(id)
{
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;
    public ICollection<Observation>? Observations { get; set; } = observations;

    public Antecedent(
    Guid id,
    string name,
    string description)
        : this(id, name, description, [])
    {

    }

    public Antecedent(Guid id)
        :this(id, string.Empty, string.Empty)
    {
        
    }

    public Antecedent() 
        : this(Guid.NewGuid())
    {

    }
}
