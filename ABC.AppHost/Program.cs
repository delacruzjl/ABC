using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var dbNameKey = "databaseName";
var databaseName = builder.Configuration[$"Parameters:{dbNameKey}"];
var databaseNameParameter = builder.AddParameter(dbNameKey);
var databasePasswordParameter = builder.AddParameter("postgres-password");

var postgres = builder
    .AddAzurePostgresFlexibleServer("postgres");

if (!builder.ExecutionContext.IsPublishMode)
{
    postgres.RunAsContainer();
}

var db = postgres.AddDatabase(databaseName!);
    
var managementApi = builder
    .AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
    .WithEnvironment(dbNameKey, databaseNameParameter)
    .WithReference(db)
    .WaitFor(db);

if (builder.ExecutionContext.IsPublishMode)
{
    var insights = builder.AddAzureApplicationInsights("insights");

    managementApi
        .WithReference(insights!)
        .WaitFor(insights!);
}

builder.AddNpmApp("react", "../ABC.React")
    .WithReference(managementApi)
    .WaitFor(managementApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
