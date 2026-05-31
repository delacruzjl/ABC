using ABC.PostGreSQL;
using HotChocolate;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ABC.Management.Api.Types;

public class Language
{
    [Mutation]
    [Authorize]
    [GraphQLDescription("Update the preferred language for the current user")]
    public static async Task<bool> UpdatePreferredLanguage(
        string language,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory)
    {
        var supportedLanguages = new[] { "en", "es" };
        if (!supportedLanguages.Contains(language))
            throw new GraphQLException($"Unsupported language: {language}. Supported: {string.Join(", ", supportedLanguages)}");

        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub")
            ?? throw new GraphQLException("Unable to determine user identity.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new GraphQLException("User not found.");

        user.PreferredLanguage = language;
        await dbContext.SaveChangesAsync();
        return true;
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get the preferred language for the current user")]
    public static async Task<string> GetPreferredLanguage(
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub")
            ?? throw new GraphQLException("Unable to determine user identity.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.PreferredLanguage ?? "en";
    }
}
