using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedKernel;
using Bogus.DataSets;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class ConsequenceServiceStepDefinitions
{
    private StartupFixture fixture = StartupFixture.Instance;

    private Consequence _actual;
    private string _consequenceName;
    private IEntityService<Consequence> _sut;
    private readonly Lorem _lorem;

    public ConsequenceServiceStepDefinitions()
    {
        _lorem = new Bogus.DataSets.Lorem(locale: "en");
    }

    [Given("I have a name found in the consequence table")]
    public async Task GivenIHaveANameFoundInTheConsequenceTable()
    {
        var consequences = Enumerable
            .Range(0, 2)
            .Select(i => new Consequence(
                Guid.NewGuid(),
                $"behavior{i}",
                _lorem.Sentence()))
            .ToList();

        var _uow = fixture.Services
            .GetRequiredService<IUnitOfWork>();

        await _uow.Consequences.AddRangeAsync(consequences);
        await _uow.SaveChangesAsync();

        _consequenceName = consequences.First().Name;
    }

    [Given("I make a call to the consequence service")]
    public void GivenIMakeACallToTheConsequenceService() =>
         _sut = fixture.Services
            .GetRequiredService<IEntityService<Consequence>>();

    [When("getting consequence by name")]
    public async Task WhenGettingConsequenceByName() =>
        _actual = await _sut.GetByName(_consequenceName);


    [Then("I should receive the consequence object from the database")]
    public void ThenIShouldReceiveTheConsequenceObjectFromTheDatabase() =>
        _actual.ShouldNotBeNull();
}
