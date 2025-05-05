using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveAntecedentStepDefinitions
{
    private readonly RemoveAntecedentHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private List<RemoveAntecedentResponseCommand> _requestFakes;
    private List<BaseResponseCommand<Antecedent>> _actual;
    private readonly RemoveAntecedentHandlerDecorator _decorator;

    public RemoveAntecedentStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("an antecedent Id of {float}ddd-d{float}ff{int}-a{int}-fb{float}ab")]
    public void GivenAnAntecedentIdOfDdd_Dff_A_Fbab(Decimal p0, Decimal p1, int p2, int p3, Decimal p4)
    {
        throw new PendingStepException();
    }

    [Given("the antecedent with that Id exists in the database")]
    public void GivenTheAntecedentWithThatIdExistsInTheDatabase()
    {
        throw new PendingStepException();
    }

    [When("I send a request to Antecedent mutation for removal")]
    public void WhenISendARequestToAntecedentMutationForRemoval()
    {
        throw new PendingStepException();
    }

    [Then("handler should return true")]
    public void ThenHandlerShouldReturnTrue()
    {
        throw new PendingStepException();
    }

    [Then("Antecedent response should contain {int} error objects in array")]
    public void ThenAntecedentResponseShouldContainErrorObjectsInArray(int p0)
    {
        throw new PendingStepException();
    }

    [Given("the antecedent with that Id does not exist in the database")]
    public void GivenTheAntecedentWithThatIdDoesNotExistInTheDatabase()
    {
        throw new PendingStepException();
    }

    [Then("handler should return false")]
    public void ThenHandlerShouldReturnFalse()
    {
        throw new PendingStepException();
    }
}
