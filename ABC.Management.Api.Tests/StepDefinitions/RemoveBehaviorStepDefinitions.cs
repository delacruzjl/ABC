using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Api.Types;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveBehaviorStepDefinitions
{
    private readonly RemoveBehaviorResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private BaseResponseCommand<Behavior>? _actual;
    private readonly RemoveBehaviorHandlerDecorator _decorator;
    private Guid? _behaviorId;

    public RemoveBehaviorStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("a Behavior Id of \"(.+)\"")]
    public void GivenABehaviorIdOf(string behaviorId) =>
        _behaviorId = Guid.TryParse(behaviorId, out var id) ? id : null;

    [Given("the Behavior with that Id exists in the database")]
    public void GivenTheBehaviorWithThatIdExistsInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(1);

    [When("I send a request to Behavior mutation for removal")]
    public async Task WhenISendARequestToBehaviorMutationForRemoval()
    {
        var command = RemoveBehaviorResponseCommand.Create(
           new Behavior(_behaviorId!.Value));

        _actual = await _decorator.Handle(
            command!,
            async (cmd, ct) => await _sut.Handle(cmd, ct),
            CancellationToken.None);
    }

    [Then("behavior handler should return true")]
    public void ThenBehaviorHandlerShouldReturnTrue() =>
        _actual?.Errors.ShouldBeEmpty();

    [Then("Behavior response should contain {int} error objects in array")]
    public void ThenBehaviorResponseShouldContainErrorObjectsInArray(int errorCount) =>
        _actual?.Errors.Count.ShouldBe(errorCount);

    [Given("the Behavior with that Id does not exist in the database")]
    public void GivenTheBehaviorWithThatIdDoesNotExistInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(0);

    [Then("behavior handler should return false")]
    public void ThenBehaviorHandlerShouldReturnFalse() =>
        _actual?.Errors.ShouldNotBeEmpty();
}
