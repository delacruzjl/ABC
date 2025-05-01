using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Bogus.DataSets;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class AntecedentRepositoryStepDefinitions
{
    private StartupFixture fixture = StartupFixture.Instance;

    private IUnitOfWork _uow;
    private int _actual;
    private int _expected;
    private Guid _antecedentId;
    private string _antecedentName;
    private readonly Lorem _lorem;

    public AntecedentRepositoryStepDefinitions()
    {
        _lorem = new Bogus.DataSets.Lorem(locale: "en");
    }

    [Given("I have a unit of work")]
    public void GivenIHaveAUnitOfWork()
    {
        _uow = fixture.Services
            .GetRequiredService<IUnitOfWork>();
    }

    [Given("{int} rows in the antecedent table")]
    public async Task GivenRowsInTheAntecedentTable(int rows)
    {
        var antecedents = await _uow.Antecedents.GetAsync();

        _expected = rows;
        for (var i = antecedents.Count(); i < rows; i++)
        {
            var antecedent = new Antecedent(
                Guid.NewGuid(),
                _lorem.Word(),
                _lorem.Sentence());
            await _uow.Antecedents.AddAsync(antecedent);
        }

        await _uow.SaveChangesAsync();
    }

    [When("I request antecedents by an existing id from the database")]
    public async Task WhenIRequestAntecedentsByAnExistingIdFromTheDatabase()
    {
        var antecedents = await _uow.Antecedents.GetAsync();
        _antecedentId = antecedents.First().Id;
    }

    [Then("I should receive the antecedents by id from the database")]
    public async Task ThenIShouldReceiveTheAntecedentsByIdFromTheDatabase()
    {
        var antecedent = await _uow.Antecedents.FindAsync(_antecedentId);
        antecedent.Should().NotBeNull();
    }

    [When("I delete an antecedent from the database")]
    public async Task WhenIDeleteAnAntecedentFromTheDatabase()
    {
        await _uow.Antecedents.RemoveAsync(_antecedentId);
    }

    [Then("I should receive an exception indicating antecedents not found When deleting")]
    public async Task ThenIShouldReceiveAnExceptionIndicatingAntecedentsNotFoundWhenDeleting()
    {
        Func<Task> act = async () => await _uow.Antecedents.RemoveAsync(_antecedentId);
        await act.Should().ThrowAsync<DataException>();
    }


    [When("Save changes in the unit of work")]
    public async Task WhenSaveChangesInTheUnitOfWork()
    {
        await _uow.SaveChangesAsync();
    }

    [Then("I should not find the antecedent in the database")]
    public void ThenIShouldNotFindTheAntecedentInTheDatabase()
    {
        Func<Task> act = async () => await _uow.Antecedents.FindAsync(_antecedentId);
        act.Should().ThrowAsync<DataException>();
    }


    [When("I request antecedents by a non-existing id from the database")]
    public void WhenIRequestAntecedentsByANon_ExistingIdFromTheDatabase()
    {
        _antecedentId = Guid.NewGuid();
    }

    [Then("I should receive an exception indicating antecedents not found")]
    public void ThenIShouldReceiveAnExceptionIndicatingAntecedentsNotFound()
    {
        Func<Task> act =async  () => await _uow.Antecedents.FindAsync(_antecedentId);
        act.Should().ThrowAsync<DataException>();
    }


    [When("I request all antecedents from the database")]
    public async Task WhenIRequestAllAntecedentsFromTheDatabase()
    {
        var data = await _uow.Antecedents.GetAsync();
        _actual = data.Count();
    }

    [Then("I should receive all the antecedents from the database")]
    public void ThenIShouldReceiveAllTheAntecedentsFromTheDatabase()
    {
        _actual.Should().Be(_expected);
    }

    [When("I request antecedents by an existing name from the database")]
    public async Task WhenIRequestAntecedentsByAnExistingNameFromTheDatabase()
    {
        var antecedents = await _uow.Antecedents.GetAsync();
        _antecedentName = antecedents.First().Name;

    }

    [Then("I should receive the antecedents by name from the database")]
    public async Task ThenIShouldReceiveTheAntecedentsByNameFromTheDatabase()
    {
        var antecedents = await _uow.Antecedents
            .GetAsync(a => a.Name == _antecedentName);
            
        antecedents.SingleOrDefault().Should().NotBeNull();
    }

}
