using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using ABC.SharedEntityFramework;

namespace ABC.PostgreSQL.Tests.StepDefinitions;

[Binding]
public class AntecedentServiceStepDefinitions
{
    private readonly AntecedentService _sut;
    private string _expectedName = string.Empty;
    private Antecedent _actual;

    private readonly IEnumerable<Antecedent> _antecedents =
        Enumerable.Range(0, 5)
        .Select(i => new Antecedent(
            Guid.NewGuid(),
            $"Antecedent{i}",
            $"description for {i}"));


    public AntecedentServiceStepDefinitions()
    {
        var uow = StartupFixture.Instance.Services
            .GetRequiredService<IUnitOfWork>();

        Action action = async () =>
        {
            await uow.Antecedents.AddRangeAsync(_antecedents);
            _ = await uow.SaveChangesAsync();
        };

        action.Invoke();
        _sut = new AntecedentService(uow);
    }

    [Given("I have a name found in the antecedent table")]
    public void GivenIHaveANameFoundInTheAntecedentTable() =>
        _expectedName = _antecedents.First().Name;

    [Given("I make a call to the antecedent service")]
    public async Task GivenIMakeACallToTheAntecedentService() =>
        _actual = await _sut.GetByName(_expectedName);

    [Then("I should receive the antecedent object from the database")]
    public void ThenIShouldReceiveTheAntecedentObjectFromTheDatabase() =>
        _actual.ShouldNotBeNull();
}
