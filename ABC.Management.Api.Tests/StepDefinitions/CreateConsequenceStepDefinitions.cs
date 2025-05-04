using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedKernel;
using FakeItEasy;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class CreateConsequenceStepDefinitions
{
    private readonly IEntityService<Consequence> _entityService;
    private readonly CreateConsequenceResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private List<CreateConsequenceResponseCommand> _requestFakes;
    private List<BaseResponseCommand<Consequence>> _actual;
    private readonly CreateConsequenceHandlerDecorator _decorator;

    public CreateConsequenceStepDefinitions(StartupFixture fixture)
    {
        _requestFakes = new();
        _actual = new();
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var validator = fixture.Services.GetRequiredService<IValidator<Consequence>>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();
        _entityService = fixture.Services.GetRequiredService<IEntityService<Consequence>>();

        _sut = new(_uowFake);
        _decorator = new(validator, logger);
    }

    [Given("the following consequence data:")]
    public void GivenTheFollowingConsequenceData(DataTable dataTable) =>
        _requestFakes.AddRange(dataTable.Rows.Select(row =>
            CreateConsequenceResponseCommand.Create(
                row["Name"],
                row["Description"])));

    [Given("calls to the Consequence service by name returns null")]
    public void GivenCallsToTheConsequenceServiceByNameReturnsNull() =>
        A.CallTo(() => _entityService.GetByName(A<string>.Ignored, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(default(Consequence)));

    [Given("the Consequence should be saved in the database")]
    public void GivenTheConsequenceShouldBeSavedInTheDatabase() =>
        A.CallTo(() =>
           _uowFake.SaveChangesAsync())
        .Returns(Task.FromResult(1));

    [When("I send a request to Consequence mutation")]
    public async Task WhenISendARequestToConsequenceMutation()
    {
        foreach (var r in _requestFakes)
        {
            var response = await _decorator.Handle(
                r,
                async (commandRequest, ct) => await _sut.Handle(commandRequest, ct),
                CancellationToken.None);

            _actual.Add(response);
        }
    }         

    [Then("the consequence should call the handler the same amount of times as the data I sent")]
    public void ThenTheConsequenceShouldCallTheHandlerTheSameAmountOfTimesAsTheDataISent() =>
        A.CallTo(() =>
            _uowFake.Consequences.AddAsync(A<Consequence>.Ignored, A<CancellationToken>.Ignored))
        .MustHaveHappened(_requestFakes.Count, Times.Exactly);

    [Then("Consequence response should contain {int} error objects in array")]
    public void ThenConsequenceResponseShouldContainErrorObjectsInArray(
        int errorCount) =>
        _actual.ShouldAllBe(x => x.Errors.Count == errorCount,
            string.Join(", ", _actual.SelectMany(e => e.Errors).Select(e => e.Message)));


    [Given(@"a Consequence object with name: (\w+) and description: (\w+)")]
    public void GivenAConsequenceObjectWithNameJoseAndDescriptionTest(
        string name,
        string description) =>
        CreateConsequenceResponseCommand.Create(name, description);

    [Given("the SaveChanges method does not affect any consequence rows")]
    public void GivenTheSaveChangesMethodDoesNotAffectAnyConsequenceRows() =>
        A.CallTo(() =>
           _uowFake.SaveChangesAsync())
        .Returns(Task.FromResult(0));
}
