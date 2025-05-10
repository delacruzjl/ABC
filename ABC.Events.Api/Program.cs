using ABC.Management.Domain.Validators;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;


[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.AddServiceDefaults();

        var services = builder.Services;

        // services.AddValidatorsFromAssemblyContaining<AntecedentValidator>();
        services.AddMediator(mediatorOptions =>
        {
            mediatorOptions.ServiceLifetime = ServiceLifetime.Scoped;
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

        // Configure the HTTP request pipeline.
        app.MapGraphQL();

        app.RunWithGraphQLCommands(args);
    }
}