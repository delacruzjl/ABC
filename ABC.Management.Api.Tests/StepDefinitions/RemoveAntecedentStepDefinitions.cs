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
using System.Threading.Tasks;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class RemoveAntecedentStepDefinitions
{
    private readonly RemoveAntecedentHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private List<RemoveAntecedentResponseCommand> _requestFakes;
    private BaseResponseCommand<Antecedent> _actual;
    private readonly RemoveAntecedentHandlerDecorator _decorator;
    private Guid? _antecedentId;

    public RemoveAntecedentStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();

        _sut = new(_uowFake);
        _decorator = new(logger);
    }

    [Given("an antecedent Id of \"(.+)\"")]
    public void GivenAnAntecedentIdOf(string antecedentId) =>
        _antecedentId = Guid.TryParse(antecedentId, out var id) ? id : null;

    [Given("the antecedent with that Id exists in the database")]
    public void GivenTheAntecedentWithThatIdExistsInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(1);

    [When("I send a request to Antecedent mutation for removal")]
    public async Task WhenISendARequestToAntecedentMutationForRemoval()
    {
        var command = RemoveAntecedentResponseCommand.Create(
            new Antecedent(_antecedentId!.Value));

        _actual = await _decorator.Handle(
            command!,
            async (cmd, ct) => await _sut.Handle(cmd, ct),
            CancellationToken.None);
    }

    [Then("handler should return true")]
    public void ThenHandlerShouldReturnTrue() =>
        _actual.Errors.ShouldBeEmpty();

    [Then("Antecedent response should contain {int} error objects in array")]
    public void ThenAntecedentResponseShouldContainErrorObjectsInArray(int errorCount) =>
        _actual.Errors.Count.ShouldBe(errorCount);

    [Given("the antecedent with that Id does not exist in the database")]
    public void GivenTheAntecedentWithThatIdDoesNotExistInTheDatabase() =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(0);

    [Then("handler should return false")]
    public void ThenHandlerShouldReturnFalse() =>
        _actual.Errors.ShouldNotBeEmpty();
}
