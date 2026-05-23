namespace ABC.Management.Domain.Entities;

public class AntecedentEntry(
    Guid id,
    Antecedent antecedent,
    Observation observation,
    string whereAt,
    string how) : Entity(id)
{
    public Antecedent Antecedent { get; init; } = antecedent;
    public Observation Observation { get; init; } = observation;
    public string Where { get; init; } = whereAt;
    public string How { get; init; } = how;

    public AntecedentEntry(Guid id, Antecedent antecedent, Observation observation)
        : this(id, antecedent, observation, string.Empty, string.Empty)
    {

    }
}