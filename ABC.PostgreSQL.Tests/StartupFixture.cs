using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace ABC.PostgreSQL.Tests;

public sealed partial class PostgreSqlContainerFixture(ITestOutputHelper testOutputHelper)
    : DbContainerTest<PostgreSqlBuilder, PostgreSqlContainer>(testOutputHelper)
{
    public override DbProviderFactory DbProviderFactory => NpgsqlFactory.Instance;

    protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
    {
        return builder
            .WithImage("postgres:15.1");
//            .WithResourceMapping("Chinook_PostgreSql_AutoIncrementPKs.sql", "/docker-entrypoint-initdb.d/");
    }
}



public class StartupFixture
    : IClassFixture<PostgreSqlContainerFixture>
{
    //public static StartupFixture Instance { get; } = new StartupFixture();

    public IServiceProvider Services;

    private readonly PostgreSqlContainer _container;

    public StartupFixture(PostgreSqlContainerFixture fixture)
    {
        var collection = new ServiceCollection();

        //_container = new PostgreSqlBuilder()
        //    .Build();

        _container = fixture.Container;

        collection.AddDbContextFactory<ABCContext>(options =>
            options.UseNpgsql(
                _container.GetConnectionString(),(opt) => opt.EnableRetryOnFailure()),
                ServiceLifetime.Singleton);

        collection.AddTransient<IUnitOfWork, UnitOfWork>();
        collection.AddTransient<IEntityService<Antecedent>, AntecedentService>();
        collection.AddTransient<IEntityService<Behavior>, BehaviorService>();
        collection.AddTransient<IEntityService<Consequence>, ConsequenceService>();

        Services = collection.BuildServiceProvider();
    }
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var context = Services.GetRequiredService<ABCContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}
