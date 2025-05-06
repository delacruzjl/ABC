using ABC.Management.Domain.Entities;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using ABC.SharedKernel;
using FakeItEasy;
using FluentValidation;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Assist;
using Xunit;

namespace ABC.Management.Api.Tests;

public class StartupFixture : IAsyncLifetime
{
    public IServiceProvider Services;

    public StartupFixture()
    {
        ServiceCollection collection = new();

        var uowFake = CreateUnitOfWorkFake();
        var mediatorFake = CreateMediatorFake();
        var resolverContext = CreateResolverContextFake();
        var antecedentService = CreateEntityService<Antecedent>();
        var behaviorService = CreateEntityService<Behavior>();
        var consequenceService = CreateEntityService<Consequence>();
        var childService = CreateEntityService<Child>();

        collection.AddLogging();
        collection.AddTransient(_ => uowFake);
        collection.AddTransient(_ => mediatorFake);
        collection.AddTransient(_ => resolverContext);
        collection.AddTransient(_ => antecedentService);
        collection.AddTransient(_ => behaviorService);
        collection.AddTransient(_ => consequenceService);
        collection.AddTransient(_ => childService);

        collection
            .AddValidatorsFromAssemblyContaining<AntecedentValidator>(
                lifetime: ServiceLifetime.Transient);

        Services = collection.BuildServiceProvider();
    }

    private IEntityService<T> CreateEntityService<T>() where T : Entity =>
        A.Fake<IEntityService<T>>();

    private IResolverContext CreateResolverContextFake() =>
        A.Fake<IResolverContext>();

    private IMediator CreateMediatorFake() =>
        A.Fake<IMediator>();

    private IUnitOfWork CreateUnitOfWorkFake() =>
        A.Fake<IUnitOfWork>();

    public Task DisposeAsync() =>
    Task.CompletedTask;

    public Task InitializeAsync() =>
        Task.CompletedTask;
}
