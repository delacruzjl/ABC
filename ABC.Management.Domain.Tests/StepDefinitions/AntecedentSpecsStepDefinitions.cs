using System;
using ABC.Management.Domain.Entities;
using Shouldly;
using Reqnroll;

namespace ABC.Management.Domain.Tests.StepDefinitions;

[Binding]
public class AntecedentSpecsStepDefinitions
{
    private Antecedent? _firstAntecedent;
    private Antecedent? _secondAntecedent;
    private bool _actual = false;

    [Given(@"An antecedent with Id {string}")]
    public void GivenAnAntecedentWithId(string id)
    {
        _firstAntecedent = new(Guid.Parse(id));
    }

    [Given(@"another antecedent with Id {string}")]
    public void GivenAnotherAntecedentWithId(string id)
    {
        _secondAntecedent = new(Guid.Parse(id));
    }

    [When("Comparing objects by id")]
    public void WhenComparingObjectsById()
    {
        _actual = _firstAntecedent?.Equals(_secondAntecedent) ?? false;
    }

    [Then("Equals should be True")]
    public void ThenEqualsShouldBeTrue()
    {
        _actual.ShouldBeTrue();
    }

    [When("Comparing objects with equals operator")]
    public void WhenComparingObjectsWithEqualsOperator()
    {
        _actual = _firstAntecedent == _secondAntecedent;
    }

    [When("Comparing objects with different operator")]
    public void WhenComparingObjectsWithDifferentOperator()
    {
        _actual = _firstAntecedent != _secondAntecedent;
    }

    [Then("Different should be True")]
    public void ThenDifferentShouldBeTrue()
    {
        _actual.ShouldBeTrue();
    }
}
