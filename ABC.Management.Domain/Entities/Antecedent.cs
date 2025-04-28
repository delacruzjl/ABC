namespace ABC.Management.Domain.Entities;

public class Antecedent(
    Guid id,
    string name,
    string description) : Entity(id)
{
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;

    public Antecedent(Guid id)
        :this(id, string.Empty, string.Empty)
    {
        
    }

    public Antecedent() 
        : this(Guid.NewGuid())
    {

    }
}
