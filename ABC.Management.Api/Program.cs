using ABC.Management.Api.Types;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using ABC.PostGreSQL.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddABCPostGreSQL();

var services = builder.Services;

services.AddValidatorsFromAssemblyContaining<AntecedentValidator>();
services.AddMediator(options =>
    options.ServiceLifetime = ServiceLifetime.Scoped);

services
    .AddGraphQLServer()
    .ModifyRequestOptions(o =>
        o.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddTypes()
    .AddTypeExtension<MutationRemoveResolvers>()
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