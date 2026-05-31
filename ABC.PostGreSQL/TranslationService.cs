using ABC.SharedKernel;
using ABC.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace ABC.PostGreSQL;

public class TranslationService(IDbContextFactory<ABCContext> dbContextFactory) : ITranslationService
{
    public async Task<(string Name, string Description)?> GetTranslationAsync(
        TranslatableEntityType entityType,
        Guid entityId,
        string language,
        CancellationToken cancellationToken = default)
    {
        if (language == "en")
            return null;

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var translation = await context.EntityTranslations
            .AsNoTracking()
            .FirstOrDefaultAsync(t =>
                    t.EntityType == entityType &&
                    t.EntityId == entityId &&
                    t.Language == language,
                cancellationToken);

        if (translation is null)
            return null;

        return (translation.Name, translation.Description);
    }

    public async Task<Dictionary<Guid, (string Name, string Description)>> GetTranslationsAsync(
        TranslatableEntityType entityType,
        IEnumerable<Guid> entityIds,
        string language,
        CancellationToken cancellationToken = default)
    {
        if (language == "en")
            return new Dictionary<Guid, (string Name, string Description)>();

        var ids = entityIds.ToList();
        if (ids.Count == 0)
            return new Dictionary<Guid, (string Name, string Description)>();

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var translations = await context.EntityTranslations
            .AsNoTracking()
            .Where(t =>
                t.EntityType == entityType &&
                ids.Contains(t.EntityId) &&
                t.Language == language)
            .ToDictionaryAsync(
                t => t.EntityId,
                t => (t.Name, t.Description),
                cancellationToken);

        return translations;
    }
}
