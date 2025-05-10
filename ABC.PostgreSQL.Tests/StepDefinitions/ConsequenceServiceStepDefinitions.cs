using ABC.Management.Domain.Entities;
using ABC.SharedKernel;
using Bogus.DataSets;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Linq;
using System.Threading.Tasks;
using ABC.SharedEntityFramework;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class ConsequenceServiceStepDefinitions
{
    private readonly IUnitOfWork _uowFake;
    private readonly IEntityService<Consequence> _consequenceService;
    private Consequence _actual;
    private string _consequenceName;
    private IEntityService<Consequence> _sut;
    private readonly Lorem _lorem;

    public ConsequenceServiceStepDefinitions()
    {
        StartupFixture fixture = StartupFixture.Instance;

        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        _consequenceService = fixture.Services
            .GetRequiredService<IEntityService<Consequence>>();

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

        var _uow = _uowFake;

        await _uow.Consequences.AddRangeAsync(consequences);
        await _uow.SaveChangesAsync();

        _consequenceName = consequences.First().Name;
    }

    [Given("I make a call to the consequence service")]
    public void GivenIMakeACallToTheConsequenceService() =>
         _sut = _consequenceService;

    [When("getting consequence by name")]
    public async Task WhenGettingConsequenceByName() =>
        _actual = await _sut.GetByName(_consequenceName);


    [Then("I should receive the consequence object from the database")]
    public void ThenIShouldReceiveTheConsequenceObjectFromTheDatabase() =>
        _actual.ShouldNotBeNull();
}
