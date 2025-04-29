using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace ABC.PostgreSQL.Tests;

public class StartupFixture 
{
    public static StartupFixture Instance { get; } = new StartupFixture();

    public IServiceProvider Services;

    private readonly PostgreSqlContainer _container;

    public StartupFixture()
    {
        var collection = new ServiceCollection();

        _container = new PostgreSqlBuilder()
            .Build();

        collection.AddDbContextFactory<ABCContext>(options =>
            options.UseNpgsql(_container.GetConnectionString()));

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
