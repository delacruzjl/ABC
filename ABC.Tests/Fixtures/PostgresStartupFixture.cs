using System;
using System.Threading.Tasks;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace ABC.Tests.Fixtures;

public class PostgresStartupFixture : IAsyncLifetime
{
    public IServiceProvider Services { get; private set; }

    private readonly PostgreSqlContainer _container;

    public PostgresStartupFixture()
    {
        _container = new PostgreSqlBuilder("postgres:15.1")
            .Build();

        var collection = new ServiceCollection();

        collection.AddDbContextFactory<ABCContext>(options =>
            options.UseNpgsql(
                _container.GetConnectionString(), (opt) => opt.EnableRetryOnFailure())
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)),
                ServiceLifetime.Singleton);

        collection.AddTransient<IUnitOfWork, UnitOfWork>();
        collection.AddTransient<IEntityService<Antecedent>, AntecedentService>();
        collection.AddTransient<IEntityService<Behavior>, BehaviorService>();
        collection.AddTransient<IEntityService<Consequence>, ConsequenceService>();
        collection.AddTransient<ITranslationService, TranslationService>();

        Services = collection.BuildServiceProvider();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var context = Services.GetRequiredService<ABCContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
