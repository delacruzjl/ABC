using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedKernel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class BehaviorServiceStepDefinitions
{
    private StartupFixture fixture = StartupFixture.Instance;

    private Behavior _actual;
    private string _behaviorName;
    private IEntityService<Behavior> _sut;

    [Given("I have a name found in the behavior table")]
    public async Task GivenIHaveANameFoundInTheBehaviorTable()
    {
        var behaviors = Enumerable
            .Range(0, 2)
            .Select(i => new Behavior(
                Guid.NewGuid(),
                $"behavior{i}",
                Faker.Lorem.Sentence(1 + i)))
            .ToList();

        var _uow = fixture.Services
            .GetRequiredService<IUnitOfWork>();

        await _uow.Behaviors.AddRangeAsync(behaviors);
        await _uow.SaveChangesAsync();

        _behaviorName = behaviors.First().Name;
    }

    [Given("I make a call to the behavior service")]
    public void GivenIMakeACallToTheBehaviorService()
    {
        _sut = fixture.Services
            .GetRequiredService<IEntityService<Behavior>>();
    }

    [When("Get behavior by name")]
    public async Task WhenGetBehaviorByName()
    {
        _actual = await _sut.GetByName(_behaviorName);
    }

    [Then("I should receive the behavior object from the database")]
    public void ThenIShouldReceiveTheBehaviorObjectFromTheDatabase()
    {
        _actual.Should().NotBeNull();
    }
}
