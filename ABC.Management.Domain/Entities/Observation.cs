namespace ABC.Management.Domain.Entities;

public class Observation(
    Guid _id,
    List<Antecedent> _antecedents,
    List<Behavior> _behaviors,
    List<Consequence> _consequences,
    Child _child,
    string _notes) : Entity(_id)
{
    public IReadOnlyCollection<Antecedent> Antecedents => _antecedents.AsReadOnly();
    public IReadOnlyCollection<Behavior> Behaviors => _behaviors.AsReadOnly();
    public IReadOnlyCollection<Consequence> Consequences => _consequences.AsReadOnly();
    public Child Child { get; private set; } = _child;
    public string Notes { get; private set; } = _notes;

    public Observation()
        : this(
              Guid.NewGuid(),
              [],
              [],
              [],
              new(),
              string.Empty)
    {

    }
}
