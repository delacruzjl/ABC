using ABC.Management.Api.Decorators;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using ABC.PostGreSQL.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

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
                typeof(RemoveChildHandlerDecorator)
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
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ABCContext>();

            await context.Database.MigrateAsync();
        }

        // Configure the HTTP request pipeline.
        app.MapGraphQL();

        app.RunWithGraphQLCommands(args);
    }
}