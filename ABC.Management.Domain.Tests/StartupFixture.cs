namespace ABC.Management.Domain.Tests;

public class StartupFixture : IAsyncLifetime
{
    public IServiceProvider Services;

    public StartupFixture()
    {
        var collection = new ServiceCollection();

        var antecedentsFake = CreateServiceFake<Antecedent>();
        var behaviorsFake = CreateServiceFake<Behavior>();
        
        collection.AddTransient(_ => antecedentsFake);
        collection.AddTransient(_ => behaviorsFake);

        Services = collection.BuildServiceProvider();
    }

    private static IEntityService<T> CreateServiceFake<T>() where T : Entity =>
        A.Fake<IEntityService<T>>();
    
    public Task DisposeAsync()=>
        Task.CompletedTask;

    public Task InitializeAsync() =>
        Task.CompletedTask;
}
