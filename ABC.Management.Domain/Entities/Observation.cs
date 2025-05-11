namespace ABC.Management.Domain.Entities;

public class Observation(Guid id) : Entity(id)
{
    private readonly List<Antecedent> _antecedents = new();
    private readonly List<Behavior> _behaviors = new();
    private readonly List<Consequence> _consequences = new();

    public IReadOnlyCollection<Antecedent> Antecedents => _antecedents.AsReadOnly();
    public IReadOnlyCollection<Behavior> Behaviors => _behaviors.AsReadOnly();
    public IReadOnlyCollection<Consequence> Consequences => _consequences.AsReadOnly();
    public Child Child { get; private set; }
    public string Notes { get; private set; } 
}
