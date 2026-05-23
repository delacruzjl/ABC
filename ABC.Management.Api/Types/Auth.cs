using ABC.Management.Api.Settings;
using ABC.PostGreSQL;
using Google.Apis.Auth;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABC.Management.Api.Types;

public enum ExternalAuthProvider
{
    Google,
    AzureEntra
}

public class Auth
{
    [Mutation]
    [GraphQLDescription("Login with email and password")]
    public static async Task<AuthPayload> Login(
        string email,
        string password,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtOptions)
    {
        var user = await userManager.FindByEmailAsync(email)
            ?? throw new GraphQLException("Invalid email or password.");

        if (!user.IsActive)
            throw new GraphQLException("This account has been deactivated. Contact an administrator.");

        var valid = await userManager.CheckPasswordAsync(user, password);
        if (!valid)
            throw new GraphQLException("Invalid email or password.");

        var token = await GenerateToken(user, userManager, jwtOptions.Value);
        var roles = await userManager.GetRolesAsync(user);
        return new AuthPayload(token, user.Email!, roles.ToList());
    }

    [Mutation]
    [GraphQLDescription("Login with an external identity provider (Google or Azure Entra)")]
    public static async Task<AuthPayload> ExternalLogin(
        ExternalAuthProvider provider,
        string idToken,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions,
        IOptions<ExternalAuthSettings> externalAuthOptions)
    {
        var externalAuth = externalAuthOptions.Value;

        var email = provider switch
        {
            ExternalAuthProvider.Google => await ValidateGoogleToken(idToken, externalAuth.Google),
            ExternalAuthProvider.AzureEntra => await ValidateAzureEntraToken(idToken, externalAuth.AzureEntra),
            _ => throw new GraphQLException("Unsupported provider.")
        };

        var user = await userManager.FindByEmailAsync(email);

        if (user is not null && !user.IsActive)
            throw new GraphQLException("This account has been deactivated. Contact an administrator.");

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new GraphQLException($"Failed to create user: {errors}");
            }

            await userManager.AddToRoleAsync(user, "User");
        }

        var providerName = provider.ToString();
        var logins = await userManager.GetLoginsAsync(user);
        if (!logins.Any(l => l.LoginProvider == providerName))
        {
            await userManager.AddLoginAsync(user,
                new UserLoginInfo(providerName, email, providerName));
        }

        var token = await GenerateToken(user, userManager, jwtOptions.Value);
        var roles = await userManager.GetRolesAsync(user);
        return new AuthPayload(token, user.Email!, roles.ToList());
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get the currently authenticated user")]
    public static async Task<AuthPayload> Me(
        ClaimsPrincipal claimsPrincipal,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new GraphQLException("Not authenticated.");

        var user = await userManager.FindByIdAsync(userId)
            ?? throw new GraphQLException("User not found.");

        var token = await GenerateToken(user, userManager, jwtOptions.Value);
        var roles = await userManager.GetRolesAsync(user);
        return new AuthPayload(token, user.Email!, roles.ToList());
    }

    [Query]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Get all users with management info (admin only)")]
    public static async Task<List<UserInfo>> GetUsers(
        UserManager<ApplicationUser> userManager,
        ABCContext dbContext)
    {
        var users = userManager.Users.ToList();
        var childUserIds = await dbContext.Children
            .Select(c => c.UserId)
            .Distinct()
            .ToListAsync();

        var observationUserIds = await dbContext.Children
            .Where(c => c.Observations.Any())
            .Select(c => c.UserId)
            .Distinct()
            .ToListAsync();

        var result = new List<UserInfo>();
        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            result.Add(new UserInfo(
                u.Id,
                u.Email!,
                roles.ToList(),
                u.IsActive,
                childUserIds.Contains(u.Id),
                observationUserIds.Contains(u.Id)));
        }
        return result;
    }

    [Mutation]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Deactivate a user account (admin only)")]
    public static async Task<UserInfo> DeactivateUser(
        string userId,
        ClaimsPrincipal claimsPrincipal,
        UserManager<ApplicationUser> userManager)
    {
        var currentUserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == currentUserId)
            throw new GraphQLException("You cannot deactivate your own account.");

        var user = await userManager.FindByIdAsync(userId)
            ?? throw new GraphQLException("User not found.");

        if (!user.IsActive)
            throw new GraphQLException("User is already deactivated.");

        user.IsActive = false;
        await userManager.UpdateAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        return new UserInfo(user.Id, user.Email!, roles.ToList(), user.IsActive, false, false);
    }

    [Mutation]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Reactivate a user account (admin only)")]
    public static async Task<UserInfo> ReactivateUser(
        string userId,
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new GraphQLException("User not found.");

        if (user.IsActive)
            throw new GraphQLException("User is already active.");

        user.IsActive = true;
        await userManager.UpdateAsync(user);

        var roles = await userManager.GetRolesAsync(user);
        return new UserInfo(user.Id, user.Email!, roles.ToList(), user.IsActive, false, false);
    }

    [Mutation]
    [Authorize(Roles = ["Admin"])]
    [GraphQLDescription("Delete a user account — blocked if user has children (admin only)")]
    public static async Task<bool> DeleteUser(
        string userId,
        ClaimsPrincipal claimsPrincipal,
        UserManager<ApplicationUser> userManager,
        ABCContext dbContext)
    {
        var currentUserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == currentUserId)
            throw new GraphQLException("You cannot delete your own account.");

        var user = await userManager.FindByIdAsync(userId)
            ?? throw new GraphQLException("User not found.");

        var hasChildren = await dbContext.Children.AnyAsync(c => c.UserId == userId);
        if (hasChildren)
            throw new GraphQLException("Cannot delete a user with children. Deactivate the account instead.");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new GraphQLException($"Failed to delete user: {errors}");
        }

        return true;
    }

    [Mutation]
    [Authorize]
    [GraphQLDescription("Set the default child for the current user")]
    public static async Task<Guid?> SetDefaultChild(
        Guid? childId,
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory,
        ILogger<Auth> logger)
    {
        var userId = GetUserId(claimsPrincipal);
        logger.LogWarning("SetDefaultChild: userId={UserId}, childId={ChildId}", userId, childId);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // Diagnostic: check what users exist
        var userCount = await dbContext.Users.CountAsync();
        var userIds = await dbContext.Users.Select(u => u.Id).Take(5).ToListAsync();
        logger.LogWarning("SetDefaultChild: DB has {Count} users. First IDs: {Ids}", userCount, string.Join(", ", userIds));

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new GraphQLException($"User '{userId}' not found among {userCount} users in DB.");

        if (childId.HasValue)
        {
            var childExists = await dbContext.Children
                .AnyAsync(c => c.Id == childId.Value && c.UserId == userId);
            if (!childExists)
                throw new GraphQLException("Child not found or does not belong to you.");
        }

        user.DefaultChildId = childId;
        await dbContext.SaveChangesAsync();
        return user.DefaultChildId;
    }

    [Query]
    [Authorize]
    [GraphQLDescription("Get the default child ID for the current user")]
    public static async Task<Guid?> GetDefaultChildId(
        ClaimsPrincipal claimsPrincipal,
        IDbContextFactory<ABCContext> dbContextFactory)
    {
        var userId = GetUserId(claimsPrincipal);
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.DefaultChildId;
    }

    private static string GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        // Try standard claim types - .NET's JsonWebTokenHandler may use different mappings
        return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub")
            ?? claimsPrincipal.FindFirstValue("nameid")
            ?? throw new GraphQLException("Unable to determine user identity.");
    }

    private static async Task<string> ValidateGoogleToken(
        string idToken, GoogleSettings settings)
    {
        if (string.IsNullOrEmpty(settings.ClientId))
            throw new GraphQLException("Google authentication is not configured.");

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [settings.ClientId]
                });

            if (string.IsNullOrEmpty(payload.Email))
                throw new GraphQLException("Google token does not contain an email.");

            return payload.Email;
        }
        catch (InvalidJwtException)
        {
            throw new GraphQLException("Invalid Google token.");
        }
    }

    private static async Task<string> ValidateAzureEntraToken(
        string idToken, AzureEntraSettings settings)
    {
        if (string.IsNullOrEmpty(settings.ClientId) || string.IsNullOrEmpty(settings.TenantId))
            throw new GraphQLException("Azure Entra authentication is not configured.");

        var authority = $"https://login.microsoftonline.com/{settings.TenantId}/v2.0";
        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{authority}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());

        var config = await configManager.GetConfigurationAsync();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,
            ValidateAudience = true,
            ValidAudience = settings.ClientId,
            ValidateLifetime = true,
            IssuerSigningKeys = config.SigningKeys,
            ValidateIssuerSigningKey = true
        };

        try
        {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(idToken, validationParameters, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var tid = jwtToken.Claims.FirstOrDefault(c => c.Type == "tid")?.Value;
            if (tid != settings.TenantId)
                throw new GraphQLException("Token is not from the authorized tenant.");

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value
                ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                ?? throw new GraphQLException("Azure Entra token does not contain an email.");

            return email;
        }
        catch (SecurityTokenException)
        {
            throw new GraphQLException("Invalid Azure Entra token.");
        }
    }

    internal static async Task<string> GenerateToken(
        ApplicationUser user,
        UserManager<ApplicationUser> userManager,
        JwtSettings jwt)
    {
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        foreach (var audience in jwt.Audiences)
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record AuthPayload(string Token, string Email, List<string> Roles);
public record UserInfo(
    string Id,
    string Email,
    List<string> Roles,
    bool IsActive,
    bool HasChildren,
    bool HasObservations);
