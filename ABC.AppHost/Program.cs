using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var insights = builder.Environment.IsDevelopment()
    ? builder.AddConnectionString(
        "AppInsConnectionString",
        "APPLICATIONINSIGHTS_CONNECTION_STRING")
    : builder.AddAzureApplicationInsights("insights");

var postgres = builder
    .AddPostgres("postgres")
    .WithPgWeb(pgWeb => pgWeb.WithHostPort(5050));

var dbNameKey = "databaseName";
var databaseNameParameter = builder.AddParameter(dbNameKey);

var databaseName = builder.Configuration[$"Parameters:{dbNameKey}"];

var db = postgres
    .AddDatabase(databaseName!);

var managementApi = builder.AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
    .WithEnvironment(dbNameKey, databaseNameParameter)
    .WithReference(db)
    .WithReference(insights)
    .WaitFor(db);

builder.AddNpmApp("react", "../ABC.React")
    .WithReference(managementApi)
    .WithReference(insights)
    .WaitFor(managementApi)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
