using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveConsequenceStepDefinitions
{
    private readonly RemoveConsequenceResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private BaseResponseCommand<Consequence>? _actual;
    private readonly RemoveConsequenceHandlerDecorator _decorator;
    private Guid? _consequenceId;

    public RemoveConsequenceStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("a Consequence Id of {string}")]
    public void GivenAConsequenceIdOf(string consequenceId) =>
        _consequenceId = Guid.TryParse(consequenceId, out var id) ? id : null;

    [Given("the Consequence with that Id exists in the database")]
    public void GivenTheConsequenceWithThatIdExistsInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(1);

    [When("I send a request to Consequence mutation for removal")]
    public async Task WhenISendARequestToConsequenceMutationForRemoval()
    {
        var command = RemoveConsequenceResponseCommand.Create(
            new Consequence(_consequenceId!.Value));

        _actual = await _decorator.Handle(
            command!,
            async (cmd, ct) => await _sut.Handle(cmd, ct),
            CancellationToken.None);

        A.CallTo(() => _uowFake.Consequences
            .RemoveAsync(A<Guid>.Ignored, A<CancellationToken>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Then("Consequence handler should return true")]
    public void ThenConsequenceHandlerShouldReturnTrue() =>
        _actual?.Errors.ShouldBeEmpty();

    [Given("the Consequence with that Id does not exist in the database")]
    public void GivenTheConsequenceWithThatIdDoesNotExistInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(0);

    [Then("Consequence handler should return false")]
    public void ThenConsequenceHandlerShouldReturnFalse() =>
        _actual?.Errors.ShouldNotBeEmpty();
}
