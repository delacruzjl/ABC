namespace ABC.SharedKernel.Events;

public record ObservationStarted(Guid Id, Guid? ChildId, DateTime StartedAt) : IDomainEvent;

public record AntecedentsUpdated(Guid Id, IEnumerable<Entity> Antecedents) : IDomainEvent;
public record BehaviorsUpdated(Guid Id, IEnumerable<Entity> Behaviors) : IDomainEvent;
public record ConsequencesUpdated(Guid Id, IEnumerable<Entity> Consequences) : IDomainEvent;

public record NotesUpdated(Guid Id, string Notes) : IDomainEvent;

public record ObservationEnded(Guid Id, DateTime EndedAt) : IDomainEvent;
