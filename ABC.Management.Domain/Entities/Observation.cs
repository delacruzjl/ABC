using ABC.Management.Domain.ValueObjects;
using ABC.SharedKernel.Events;
using FluentValidation.Results;

namespace ABC.Management.Domain.Entities;

public class Observation : AggregateRoot
{
    private readonly List<Antecedent> _antecedents;
    private readonly List<Behavior> _behaviors;
    private readonly List<Consequence> _consequences;

    public IReadOnlyCollection<Antecedent> Antecedents => _antecedents.AsReadOnly();
    public IReadOnlyCollection<Behavior> Behaviors => _behaviors.AsReadOnly();
    public IReadOnlyCollection<Consequence> Consequences => _consequences.AsReadOnly();
    public Child Child { get; private set; } 
    public string Notes { get; private set; } 
    public DateTimeRange When { get; private set; }
    public ObservationStatus Status { get; private set; }

    public Observation(
    Guid id,
    List<Antecedent> antecedents,
    List<Behavior> behaviors,
    List<Consequence> consequences,
    Child child,
    string notes) : base(id)
    {
        _antecedents = antecedents;
        _behaviors = behaviors;
        _consequences = consequences;

        Child = child;
        Notes = notes;
        var startedAt = DateTime.UtcNow;

        When = startedAt;
        ApplyDomainEvent(
            new ObservationStarted(id, child.Id, DateTime.UtcNow) );
    }

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

    public Observation(IEnumerable<IDomainEvent> domainEvents)
        : this() => Load(domainEvents);

    public void SetNotes(string notes) =>
         ApplyDomainEvent(new NotesUpdated(Id, notes));

    public void RegisterVitalSigns(IEnumerable<Antecedent> antecedents)
    {
        ValidateObservationStatus();
        _antecedents.AddRange(antecedents);
    }

    private void ValidateObservationStatus()
    {
        if (Status != ObservationStatus.Closed)
        {
            return;
        }

        throw new ValidationException(
            "The consultation is already closed.",
            [
                new ValidationFailure(nameof(Status), "Consultation is already closed")
            ]);

    }

    protected override void ChangeStateByUsingDomainEvent(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case ObservationStarted e:
                Id = e.Id;
                Child = new Child(e.ChildId);
                Status = ObservationStatus.Open;
                When = e.StartedAt;
                break;
            case NotesUpdated e:
                ValidateObservationStatus();
                Notes= e.Notes;
                break;
            case ObservationEnded e:
                ValidateObservationStatus();
                if (Antecedents.Count == 0 || Behaviors.Count == 0 || Consequences.Count == 0)
                {
                    throw new ValidationException(
                        "The consultation cannot be ended.",
                        [
                            new ValidationFailure(nameof(Status), "The consultation cannot be ended.")
                        ]);
                }
                Status = ObservationStatus.Closed;
                When = new DateTimeRange(When.StartedAt, DateTime.UtcNow);
                break;
        }
    }
}


public enum ObservationStatus
{
    Open,
    Closed
}