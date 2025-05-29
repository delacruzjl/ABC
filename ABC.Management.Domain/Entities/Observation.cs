using ABC.Management.Domain.ValueObjects;
using ABC.SharedKernel.Events;
using FluentValidation.Results;

namespace ABC.Management.Domain.Entities;

public class Observation : AggregateRoot
{
    //private readonly List<Antecedent> _antecedents = [];
    //private readonly List<Behavior> _behaviors = [];
    //private readonly List<Consequence> _consequences = [];

    public ICollection<Antecedent> Antecedents { get; set; } = [];
    public ICollection<Behavior> Behaviors { get; set; } = [];
    public ICollection<Consequence> Consequences { get; set; } = [];

    public Child? Child { get; private set; } 
    public string Notes { get; private set; }
    public DateTimeRange When { get; private set; } = new();
    public ObservationStatus Status { get; private set; }

    public Observation(
    Guid id,
    Child? child,
    string notes) : base(id)
    {
        Child = child;
        Notes = notes;
    }

    public Observation()
        : this(
              Guid.NewGuid(),
              null,
              string.Empty)
    {

    }

    //public void AddAntecedent(Antecedent antecedent)
    //{
    //    _antecedents.Add(antecedent);
    //}

    //public void AddBehavor(Behavior behavior)
    //{
    //    _behaviors.Add(behavior);
    //}

    //public void AddConsequence(Consequence consequence)
    //{
    //    _consequences.Add(consequence);
    //}
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
                Status = ObservationStatus.Open;
                When = new DateTimeRange(e.StartedAt, null);
                break;
            case NotesUpdated e:
                ValidateObservationStatus();
                Notes= e.Notes;
                break;
            case ObservationEnded e:
                ValidateObservationStatus();
                if ((Antecedents).Count == 0 
                    || (Behaviors).Count == 0 
                    || (Consequences).Count == 0)
                {
                    throw new ValidationException(
                        "The observation cannot be ended.",
                        [
                            new ValidationFailure(nameof(Status), "The observation cannot be ended.")
                        ]);
                }
                Status = ObservationStatus.Closed;
                When = new DateTimeRange(When.StartedAt, e.EndedAt);
                break;
            case AntecedentsUpdated e:
                ValidateObservationStatus();
                e.Antecedents.ToList()
                    .OfType<Antecedent>()
                    .ToList()
                    .ForEach(Antecedents.Add);
                break;
            case BehaviorsUpdated e:
                ValidateObservationStatus();
                e.Behaviors.ToList()
                    .OfType<Behavior>()
                    .ToList()
                    .ForEach(Behaviors.Add);
                break;
            case ConsequencesUpdated e:
                ValidateObservationStatus();
                e.Consequences.ToList()
                    .OfType<Consequence>()
                    .ToList()
                    .ForEach(Consequences.Add);
                break;
        }
    }
}


public enum ObservationStatus
{
    Open,
    Closed
}