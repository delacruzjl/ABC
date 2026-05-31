using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using ABC.SharedKernel.Enums;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ABC.Management.Api.Types;

public record TranslatedEntity(Guid Id, string Name, string Description);

public class TranslatedEntities
{
    [Query]
    [Authorize]
    [GraphQLDescription("Get antecedents with names/descriptions in the user's preferred language")]
    public static async Task<List<TranslatedEntity>> GetTranslatedAntecedents(
        IUnitOfWork uow,
        ITranslationService translationService,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        var language = await GetUserLanguage(claimsPrincipal, dbContextFactory, cancellationToken);
        var entities = await (await uow.Antecedents.GetAsync(cancellationToken)).ToListAsync(cancellationToken);
        return await ApplyTranslations(
            entities,
            TranslatableEntityType.Antecedent,
            language,
            translationService,
            entity => entity.Name,
            entity => entity.Description,
            cancellationToken);
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get behaviors with names/descriptions in the user's preferred language")]
    public static async Task<List<TranslatedEntity>> GetTranslatedBehaviors(
        IUnitOfWork uow,
        ITranslationService translationService,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        var language = await GetUserLanguage(claimsPrincipal, dbContextFactory, cancellationToken);
        var entities = await (await uow.Behaviors.GetAsync(cancellationToken)).ToListAsync(cancellationToken);
        return await ApplyTranslations(
            entities,
            TranslatableEntityType.Behavior,
            language,
            translationService,
            entity => entity.Name,
            entity => entity.Description,
            cancellationToken);
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get consequences with names/descriptions in the user's preferred language")]
    public static async Task<List<TranslatedEntity>> GetTranslatedConsequences(
        IUnitOfWork uow,
        ITranslationService translationService,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        var language = await GetUserLanguage(claimsPrincipal, dbContextFactory, cancellationToken);
        var entities = await (await uow.Consequences.GetAsync(cancellationToken)).ToListAsync(cancellationToken);
        return await ApplyTranslations(
            entities,
            TranslatableEntityType.Consequence,
            language,
            translationService,
            entity => entity.Name,
            entity => entity.Description,
            cancellationToken);
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get child conditions with names in the user's preferred language")]
    public static async Task<List<TranslatedEntity>> GetTranslatedChildConditions(
        IUnitOfWork uow,
        ITranslationService translationService,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        var language = await GetUserLanguage(claimsPrincipal, dbContextFactory, cancellationToken);
        var entities = await (await uow.ChildConditions.GetAsync(cancellationToken)).ToListAsync(cancellationToken);

        if (language == "en")
            return entities.Select(entity => new TranslatedEntity(entity.Id, entity.Name, string.Empty)).ToList();

        var translations = await translationService.GetTranslationsAsync(
            TranslatableEntityType.ChildCondition,
            entities.Select(entity => entity.Id),
            language,
            cancellationToken);

        return entities.Select(entity =>
        {
            if (translations.TryGetValue(entity.Id, out var translation))
                return new TranslatedEntity(entity.Id, translation.Name, translation.Description);

            return new TranslatedEntity(entity.Id, entity.Name, string.Empty);
        }).ToList();
    }

    private static async Task<string> GetUserLanguage(
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userId))
            return "en";

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user?.PreferredLanguage ?? "en";
    }

    private static async Task<List<TranslatedEntity>> ApplyTranslations<T>(
        List<T> entities,
        TranslatableEntityType entityType,
        string language,
        ITranslationService translationService,
        Func<T, string> nameSelector,
        Func<T, string> descriptionSelector,
        CancellationToken cancellationToken) where T : Entity
    {
        if (language == "en")
        {
            return entities
                .Select(entity => new TranslatedEntity(entity.Id, nameSelector(entity), descriptionSelector(entity)))
                .ToList();
        }

        var translations = await translationService.GetTranslationsAsync(
            entityType,
            entities.Select(entity => entity.Id),
            language,
            cancellationToken);

        return entities.Select(entity =>
        {
            if (translations.TryGetValue(entity.Id, out var translation))
                return new TranslatedEntity(entity.Id, translation.Name, translation.Description);

            return new TranslatedEntity(entity.Id, nameSelector(entity), descriptionSelector(entity));
        }).ToList();
    }
}
