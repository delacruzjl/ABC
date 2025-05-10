using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var dbNameKey = "databaseName";
var databaseName = builder.Configuration[$"Parameters:{dbNameKey}"];
var databaseNameParameter = builder.AddParameter(dbNameKey);


IResourceBuilder<ProjectResource> managementApi;
IResourceBuilder<ProjectResource> eventsApi;
// In non-publish mode (e.g., local development), use a direct PostgreSQL configuration.
// This setup allows developers to run the application locally with a lightweight database.
if (!builder.ExecutionContext.IsPublishMode)
{
    var db = builder.AddPostgres("postgres")
        .WithPgWeb(pgWeb => pgWeb.WithHostPort(5050))
        .AddDatabase(databaseName!);

    managementApi = builder
    .AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
    .WithEnvironment(dbNameKey, databaseNameParameter)
    .WithReference(db)
    .WaitFor(db);

    eventsApi = builder
        .AddProject<Projects.ABC_Events_Api>("abceventsapi")
        .WithEnvironment(dbNameKey, databaseNameParameter)
        .WithReference(db)
        .WaitFor(db);
}
else
{
    var dbFlex = builder.AddAzurePostgresFlexibleServer("postgres")
            .AddDatabase(databaseName!);

    var insights = builder.AddAzureApplicationInsights("insights");

    managementApi = builder
        .AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
        .WithEnvironment(dbNameKey, databaseNameParameter)
        .WithReference(dbFlex)
        .WaitFor(dbFlex)
        .WithReference(insights!)
        .WaitFor(insights!);

    eventsApi = builder
        .AddProject<Projects.ABC_Events_Api>("abceventsapi")
        .WithEnvironment(dbNameKey, databaseNameParameter)
        .WithReference(dbFlex)
        .WaitFor(dbFlex);
}

builder.AddNpmApp("react", "../ABC.React")
    .WithReference(managementApi)
    .WaitFor(managementApi)
    .WithReference(eventsApi)
    .WaitFor(eventsApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
