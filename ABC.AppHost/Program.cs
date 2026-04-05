using k8s.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var builder = DistributedApplication.CreateBuilder(args);

var dbNameKey = "databaseName";
var databaseName = builder.Configuration[$"Parameters:{dbNameKey}"];
var databaseNameParameter = builder.AddParameter(dbNameKey);

// var jwtKey = builder.AddParameter("jwtKey", secret: true);
var adminSeedEmail = builder.AddParameter("adminSeedEmail", secret: true);
var adminSeedPassword = builder.AddParameter("adminSeedPassword", secret: true);
var pgPassword = builder.AddParameter("pgPassword", secret: true);
var kongReactApiKey = builder.AddParameter("kongReactApiKey", secret: true);
var allowedOrigins = builder.AddParameter("allowedOrigins");

builder.Services.AddFeatureManagement();

IResourceBuilder<ProjectResource> managementApi = builder
        .AddProject<Projects.ABC_Management_Api>("abcmanagementapi")
        .WithEnvironment(dbNameKey, databaseNameParameter)
        // .WithEnvironment("Jwt__Key", jwtKey)
        .WithEnvironment("AdminSeed__Email", adminSeedEmail)
        .WithEnvironment("AdminSeed__Password", adminSeedPassword)
        .WithEnvironment("Cors__AllowedOrigins__0", allowedOrigins)
        .WithHttpsEndpoint(port: 5100, name: "kong-upstream", isProxied: false);

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
    var pg = builder.AddPostgres("postgres", password: pgPassword)
        .WithPgWeb(pgWeb => pgWeb.WithHostPort(5050));

    var db = pg.AddDatabase(databaseName!);
    var kongDb = pg.AddDatabase("kongdb", databaseName: "kong");

    managementApi = managementApi
        .WithReference(db)
        .WaitFor(db);

    // Kong Gateway — PostgreSQL-backed API gateway with key-auth
    var kong = builder.AddContainer("kong", "kong/kong-gateway", "3.9")
        .WithBindMount("./kong", "/kong/scripts")
        .WithEntrypoint("/bin/sh")
        .WithArgs("/kong/scripts/start.sh")
        .WithEnvironment("KONG_DATABASE", "postgres")
        .WithEnvironment("KONG_PG_HOST", "postgres")
        .WithEnvironment("KONG_PG_PORT", "5432")
        .WithEnvironment("KONG_PG_USER", "postgres")
        .WithEnvironment("KONG_PG_PASSWORD", pgPassword)
        .WithEnvironment("KONG_PG_DATABASE", "kong")
        .WithEnvironment("KONG_PROXY_ACCESS_LOG", "/dev/stdout")
        .WithEnvironment("KONG_ADMIN_ACCESS_LOG", "/dev/stdout")
        .WithEnvironment("KONG_PROXY_ERROR_LOG", "/dev/stderr")
        .WithEnvironment("KONG_ADMIN_ERROR_LOG", "/dev/stderr")
        .WithEnvironment("KONG_ADMIN_LISTEN", "0.0.0.0:8001")
        .WithEnvironment("KONG_ADMIN_GUI_LISTEN", "0.0.0.0:8002")
        .WithEnvironment("KONG_ADMIN_GUI_API_URL", "http://localhost:8001")
        .WithEnvironment("KONG_SSL_VERIFY", "off")
        .WithEnvironment("UPSTREAM_URL", "https://host.docker.internal:5100")
        .WithEnvironment("REACT_API_KEY", kongReactApiKey)
        .WithEnvironment("ALLOWED_ORIGINS", allowedOrigins)
        .WithHttpEndpoint(targetPort: 8000, name: "proxy")
        .WithHttpEndpoint(port: 8001, targetPort: 8001, name: "admin")
        .WithHttpEndpoint(port: 8002, targetPort: 8002, name: "manager")
        .WaitFor(pg);

    var kongProxyEndpoint = kong.GetEndpoint("proxy");

    builder.AddJavaScriptApp("react", "../ABC.React", "start")
        .WithReference(kongProxyEndpoint)
        .WaitFor(kong)
        .WithEnvironment("KONG_API_KEY", kongReactApiKey)
        .WithEnvironment("BROWSER", "none")
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();
}
else
{
    builder.AddJavaScriptApp("react", "../ABC.React", "start")
        .WithReference(managementApi)
        .WaitFor(managementApi)
        .WithEnvironment("BROWSER", "none")
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();
}

builder.Build().Run();
