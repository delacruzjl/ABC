using ABC.Management.Api.Settings;
using ABC.PostGreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ABC.Management.Api.Types;

public class DevAuth
{
    [Mutation]
    [GraphQLDescription("Dev-only: simulate external login without real token validation")]
    public static async Task<AuthPayload> DevExternalLogin(
        ExternalAuthProvider provider,
        string email,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions,
        IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
            throw new GraphQLException("DevExternalLogin is only available in development.");

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

        var token = await Auth.GenerateToken(user, userManager, jwtOptions.Value);
        var roles = await userManager.GetRolesAsync(user);
        return new AuthPayload(token, user.Email!, roles.ToList());
    }
}
