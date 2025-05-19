namespace ABC.SharedKernel.Events;

public record ObservationStarted(Guid Id, Guid ChildId, DateTime StartedAt) : IDomainEvent;

// public record AntecedentsUpdated(Guid Id, Guid AntecedentId, EntryType EntryType) : IDomainEvent;

// public record BehaviorsUpdated(Guid Id, Guid BehaviorId, EntryType EntryType) : IDomainEvent;

// public record ConsequencesUpdated(Guid Id, Guid ConsequenceId, EntryType EntryType) : IDomainEvent;

public record NotesUpdated(Guid Id, string Notes) : IDomainEvent;

public record ObservationEnded(Guid Id, DateTime EndedAt) : IDomainEvent;

//public enum EntryType
//{
//    INS, UPD, DEL
//}