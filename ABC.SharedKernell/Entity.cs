namespace ABC.SharedKernell;

public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; init; } = id;

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
