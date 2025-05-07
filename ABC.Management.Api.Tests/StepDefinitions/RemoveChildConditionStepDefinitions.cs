using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;
using System;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveChildConditionStepDefinitions
{
    private readonly RemoveChildConditionResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private BaseResponseCommand<ChildCondition>? _actual;
    private readonly RemoveChildConditionHandlerDecorator _decorator;
    private Guid? _childConditionId;

    public RemoveChildConditionStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("a ChildCondition Id of {string}")]
    public void GivenAChildConditionIdOf(string childConditionId) =>
        _childConditionId = Guid.TryParse(childConditionId, out var id) ? id : null;

    [Given("the ChildCondition with that Id exists in the database")]
    public void GivenTheChildConditionWithThatIdExistsInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(1);

    [When("I send a request to ChildCondition mutation for removal")]
    public async Task WhenISendARequestToChildConditionMutationForRemoval()
    {
        var command = RemoveChildConditionResponseCommand
            .Create(_childConditionId!.Value);

        _actual = await _decorator.Handle(
            command!,
            async (cmd, ct) => await _sut.Handle(cmd, ct),
            CancellationToken.None);
    }

    [Then("ChildCondition handler should return true")]
    public void ThenChildConditionHandlerShouldReturnTrue() =>
        _actual?.Errors.ShouldBeEmpty();

    [Then("ChildCondition response should contain {int} error objects in array")]
    public void ThenChildConditionResponseShouldContainErrorObjectsInArray(int errorCount) =>
        _actual?.Errors.Count.ShouldBe(errorCount);

    [Given("the ChildCondition with that Id does not exist in the database")]
    public void GivenTheChildConditionWithThatIdDoesNotExistInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(0);

    [Then("ChildCondition handler should return false")]
    public void ThenChildConditionHandlerShouldReturnFalse() =>
        _actual?.Errors.ShouldNotBeEmpty();
}
