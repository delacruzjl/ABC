using ABC.PostGreSQL;
using FakeItEasy;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
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

        collection.AddTransient(_ => uowFake);
        collection.AddTransient(_ => mediatorFake);
        collection.AddTransient(_ => resolverContext);

        Services = collection.BuildServiceProvider();
    }

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
