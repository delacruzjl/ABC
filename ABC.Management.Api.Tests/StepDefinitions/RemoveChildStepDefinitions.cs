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

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveChildStepDefinitions
{
    private readonly RemoveChildResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private BaseResponseCommand<Child>? _actual;
    private readonly RemoveChildHandlerDecorator _decorator;
    private Guid? _childId;

    public RemoveChildStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("a Child Id of {string}")]
    public void GivenAChildIdOf(string childId) =>
        _childId = Guid.TryParse(childId, out var id) ? id : null;

    [Given("the Child with that Id exists in the database")]
    public void GivenTheChildWithThatIdExistsInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(1);

    [When("I send a request to Child mutation for removal")]
    public async Task WhenISendARequestToChildMutationForRemoval()
    {
        var command = RemoveChildResponseCommand.Create(
           new Child(_childId!.Value));

        _actual = await _decorator.Handle(
            command!,
            async (cmd, ct) => await _sut.Handle(cmd, ct),
            CancellationToken.None);

        A.CallTo(() => _uowFake.Children
            .RemoveAsync(A<Guid>.Ignored, A<CancellationToken>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Then("child handler should return true")]
    public void ThenChildHandlerShouldReturnTrue() =>
        _actual?.Errors.ShouldBeEmpty();

    [Given("the Child with that Id does not exist in the database")]
    public void GivenTheChildWithThatIdDoesNotExistInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(0);

    [Then("child handler should return false")]
    public void ThenChildHandlerShouldReturnFalse() =>
        _actual?.Errors.ShouldNotBeEmpty();
}
