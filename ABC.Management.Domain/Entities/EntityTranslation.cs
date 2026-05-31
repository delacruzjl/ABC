using ABC.SharedKernel.Enums;

namespace ABC.Management.Domain.Entities;

public class EntityTranslation(
    Guid id,
    TranslatableEntityType entityType,
    Guid entityId,
    string language,
    string name,
    string description) : Entity(id)
{
    public TranslatableEntityType EntityType { get; init; } = entityType;
    public Guid EntityId { get; init; } = entityId;
    public string Language { get; init; } = language;
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;

    public EntityTranslation(
        TranslatableEntityType entityType,
        Guid entityId,
        string language,
        string name,
        string description)
        : this(Guid.CreateVersion7(), entityType, entityId, language, name, description)
    {
    }

    public EntityTranslation(Guid id) : this(id, default, Guid.Empty, string.Empty, string.Empty, string.Empty)
    {
    }

    public EntityTranslation() : this(Guid.CreateVersion7())
    {
    }
}
