namespace ABC.SharedKernel;

public abstract class AggregateRoot(Guid id) : Entity(id)
{
    private readonly List<IDomainEvent> _changes = new();
    public int Version { get; private set; }

    //public virtual IReadOnlyList<IDomainEvent> GetChanges()=>
    //    changes.AsReadOnly();
    
    public void ClearChanges()
    {
        _changes.Clear();
    }
    protected void ApplyDomainEvent(IDomainEvent domainEvent)
    {
        ChangeStateByUsingDomainEvent(domainEvent);
        _changes.Add(domainEvent);
        Version++;
    }

    public void Load(IEnumerable<IDomainEvent> history)
    {
        foreach (var e in history)
        {
            ApplyDomainEvent(e);
        }
        ClearChanges();
    }

    protected abstract void ChangeStateByUsingDomainEvent(IDomainEvent domainEvent);
}
