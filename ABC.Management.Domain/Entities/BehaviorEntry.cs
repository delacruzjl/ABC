using ABC.SharedKernel.Enums;

namespace ABC.Management.Domain.Entities;

public class BehaviorEntry(
    Guid id,
    Behavior behavior,
    Observation observation,
    IntensityLevel intensity,
    decimal duration) : Entity(id)
{
    public Behavior Behavior { get; init; } = behavior;
    public Observation Observation { get; init; } = observation;
    public IntensityLevel Intensity { get; init; } = intensity;
    public decimal Duration { get; init; } = duration;
    public BehaviorEntry(Guid id, Behavior behavior, Observation observation)
        : this(id, behavior, observation, IntensityLevel.Usual, 0m)
    {

    }
}