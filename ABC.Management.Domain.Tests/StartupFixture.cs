namespace ABC.Management.Domain.Tests;

public class StartupFixture : IAsyncLifetime
{
    public IServiceProvider Services;

    public StartupFixture()
    {
        var collection = new ServiceCollection();

        var antecedentsFake = CreateServiceFake<Antecedent>();
        var behaviorsFake = CreateServiceFake<Behavior>();
        var childConditionsFake = CreateServiceFake<ChildCondition>();

        collection.AddTransient(_ => antecedentsFake);
        collection.AddTransient(_ => behaviorsFake);
        collection.AddTransient(_ => childConditionsFake);

        Services = collection.BuildServiceProvider();
    }

    private static IEntityService<T> CreateServiceFake<T>() where T : Entity =>
        A.Fake<IEntityService<T>>();
    
    public Task DisposeAsync()=>
        Task.CompletedTask;

    public Task InitializeAsync() =>
        Task.CompletedTask;
}
