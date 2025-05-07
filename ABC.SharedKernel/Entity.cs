namespace ABC.SharedKernel;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; init; } = id;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public string CreatedBy { get; init; } = Thread.CurrentPrincipal?.Identity?.Name ?? "System";

    public void SetUpdatedAt() =>
        UpdatedAt = DateTime.UtcNow;

    public bool Equals(Entity? other) =>
        other?.Id.Equals(Id) ?? false;

    public override int GetHashCode() =>
        Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) =>
        left?.Id == right?.Id;
    public static bool operator !=(Entity? left, Entity? right) =>
        left?.Id != right?.Id;

    public override bool Equals(object? obj) =>
        Equals(obj as Entity);
}
