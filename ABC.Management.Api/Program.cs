using ABC.Management.Api.Decorators;
using ABC.Management.Api.Settings;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using ABC.PostGreSQL.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.AddABCPostGreSQL();

        var services = builder.Services;

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<ABCContext>()
            .AddDefaultTokenProviders();

        // JWT Settings
        var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtSection);
        var jwt = jwtSection.Get<JwtSettings>()!;

        // External Auth Settings
        var externalAuthSection = builder.Configuration.GetSection(ExternalAuthSettings.SectionName);
        services.Configure<ExternalAuthSettings>(externalAuthSection);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudiences = jwt.Audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.Key))
                };
            });

        services.AddAuthorization();

        // CORS — restrict cross-origin requests to allowed origins
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (allowedOrigins.Length > 0)
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            });
        });

        services.AddValidatorsFromAssemblyContaining<AntecedentValidator>();
        services.AddMediator((MediatorOptions mediatorOptions) =>
        {
            mediatorOptions.ServiceLifetime = ServiceLifetime.Scoped;
            mediatorOptions.PipelineBehaviors = [
                typeof(CreateAntecedentHandlerDecorator),
                typeof(CreateBehaviorHandlerDecorator),
                typeof(CreateConsequenceHandlerDecorator),
                typeof(CreateChildHandlerDecorator),
                typeof(RemoveAntecedentHandlerDecorator),
                typeof(RemoveBehaviorHandlerDecorator),
                typeof(RemoveConsequenceHandlerDecorator),
                typeof(RemoveChildHandlerDecorator),
                typeof(CreateChildConditionHandlerDecorator),
                typeof(RemoveChildConditionHandlerDecorator),
                typeof(StartObservationHandlerDecorator)
            ];
        });

        services
            .AddGraphQLServer()
            .ModifyRequestOptions(o =>
                o.IncludeExceptionDetails = builder.Environment.IsDevelopment())
            .ModifyPagingOptions(o =>
            {
                o.MaxPageSize = 100;
                o.DefaultPageSize = 25;
            })
            .ModifyCostOptions(o => o.MaxTypeCost = 2000)
            .AddTypes()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddAuthorization()
            .AddApplicationService<ILogger<ABC.Management.Api.Extensions.CorrelatedErrorFilter>>()
            .AddErrorFilter<ABC.Management.Api.Extensions.CorrelatedErrorFilter>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            await ApplyMigrationsAsync(app.Services);
            await SeedAdminAsync(app.Services, app.Configuration);
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        app.MapDefaultEndpoints();
        app.MapGraphQL();

        app.RunWithGraphQLCommands(args);
    }

    private static async Task ApplyMigrationsAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ABCContext>();
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();
        });

        await context.Database.CloseConnectionAsync();
    }

    private static async Task SeedAdminAsync(
        IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = ["Admin", "User"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = configuration["AdminSeed:Email"];
        ArgumentException.ThrowIfNullOrEmpty(adminEmail);

        var adminPassword = configuration["AdminSeed:Password"];
        ArgumentException.ThrowIfNullOrEmpty(adminPassword);
        
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}