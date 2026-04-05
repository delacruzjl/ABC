using ABC.Management.Api.Settings;
using ABC.PostGreSQL;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABC.Management.Api.Types;

public class Auth
{
    [Mutation]
    [GraphQLDescription("Register a new user account")]
    public static async Task<AuthPayload> Register(
        string email,
        string password,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new GraphQLException(errors);
        }

        await userManager.AddToRoleAsync(user, "User");

        var token = await GenerateToken(user, userManager, jwtOptions.Value);
        var roles = await userManager.GetRolesAsync(user);
        return new AuthPayload(token, user.Email!, roles.ToList());
    }

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

        var valid = await userManager.CheckPasswordAsync(user, password);
        if (!valid)
            throw new GraphQLException("Invalid email or password.");

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
    [GraphQLDescription("Get all users (admin only)")]
    public static async Task<List<UserInfo>> GetUsers(
        UserManager<ApplicationUser> userManager)
    {
        var users = userManager.Users.ToList();
        var result = new List<UserInfo>();
        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            result.Add(new UserInfo(u.Id, u.Email!, roles.ToList()));
        }
        return result;
    }

    private static async Task<string> GenerateToken(
        ApplicationUser user,
        UserManager<ApplicationUser> userManager,
        JwtSettings jwt)
    {
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
public record UserInfo(string Id, string Email, List<string> Roles);
