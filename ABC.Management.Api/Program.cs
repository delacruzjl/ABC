using ABC.Management.Api.Decorators;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using ABC.PostGreSQL.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.AddABCPostGreSQL();

        var services = builder.Services;

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
                typeof(RemoveChildConditionHandlerDecorator)
            ];
        });

        services
            .AddGraphQLServer()
            .ModifyRequestOptions(o =>
                o.IncludeExceptionDetails = builder.Environment.IsDevelopment())
            .AddTypes()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            await ApplyMigrationsAsync(app.Services);
        }

        // Configure the HTTP request pipeline.
        app.MapGraphQL();

        app.RunWithGraphQLCommands(args);
    }

    private static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ABCContext>();
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();
            await context.Database.CloseConnectionAsync();
        });
    }
}