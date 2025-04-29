using ABC.Management.Domain.Entities;
using ABC.Management.Domain.Tests;
using ABC.PostGreSQL;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class AntecedentRepositoryStepDefinitions(StartupFixture fixture)
        : IClassFixture<StartupFixture>
{
    private UnitOfWork _uow;
    private int _actual;
    private int _expected;

    [BeforeScenario]
    public async Task BeforeEach()
    {
        await fixture.InitializeAsync();
    }

    [AfterScenario]
    public async Task AfterEach()
    {
        await fixture.DisposeAsync();
    }

    [Given("I have a unit of work")]
    public void GivenIHaveAUnitOfWork()
    {
        _uow = fixture.Services
            .GetRequiredService<UnitOfWork>();
    }

    [Given("{int} rows in the antecedent table")]
    public async Task GivenRowsInTheAntecedentTable(int rows)
    {
        _expected = rows;
        for (var i = 0; i < rows; i++)
        {
            var antecedent = new Antecedent(
                Guid.NewGuid(),
                Faker.Lorem.Sentence(1+i),
                Faker.Lorem.Sentence(1 + i));
            await _uow.Antecedents.AddAsync(antecedent);
        }

        await _uow.SaveChangesAsync();
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
}
