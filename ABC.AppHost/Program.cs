using k8s.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var builder = DistributedApplication.CreateBuilder(args);

var dbNameKey = "databaseName";
var databaseName = builder.Configuration[$"Parameters:{dbNameKey}"];
var databaseNameParameter = builder.AddParameter(dbNameKey);

builder.Services.AddFeatureManagement();

IResourceBuilder<ProjectResource> managementApi = builder
        .AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
        .WithEnvironment(dbNameKey, databaseNameParameter);

var featureManager = builder.Services.BuildServiceProvider().GetRequiredService<IFeatureManager>();

// In non-publish mode (e.g., local development), use a direct PostgreSQL configuration.
// This setup allows developers to run the application locally with a lightweight database.

var useAzurePostgres = await featureManager.IsEnabledAsync("UseAzurePostgres");
var useDockerPostgres = await featureManager.IsEnabledAsync("UseDockerPostgres");
var usePgAdmin = await featureManager.IsEnabledAsync("UsePgAdmin");

if (useAzurePostgres)
{
    var dbFlex = builder.AddAzurePostgresFlexibleServer("postgres")
         .AddDatabase(databaseName!);

    var insights = builder.AddAzureApplicationInsights("insights");

    managementApi = managementApi        
        .WithReference(dbFlex)
        .WaitFor(dbFlex)
        .WithReference(insights!)
        .WaitFor(insights!);
}

if (useDockerPostgres)
{
    var db = builder.AddPostgres("postgres")
        .WithPgWeb(pgWeb => pgWeb.WithHostPort(5050))
        .AddDatabase(databaseName!);

    managementApi = managementApi
    .WithReference(db)
    .WaitFor(db);
}

builder.AddNpmApp("react", "../ABC.React")
    .WithReference(managementApi)
    .WaitFor(managementApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
