using ABC.SharedKernel.Enums;

namespace ABC.SharedKernel;

public interface ITranslationService
{
    Task<(string Name, string Description)?> GetTranslationAsync(
        TranslatableEntityType entityType,
        Guid entityId,
        string language,
        CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, (string Name, string Description)>> GetTranslationsAsync(
        TranslatableEntityType entityType,
        IEnumerable<Guid> entityIds,
        string language,
        CancellationToken cancellationToken = default);
}
