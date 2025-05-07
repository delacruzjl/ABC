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
public class CreateAntecedentResponseStepDefinitions
{
    private readonly IEntityService<Antecedent> _antecedentService;
    private readonly CreateAntecedentResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private CreateAntecedentResponseCommand? _requestFake;
    private BaseResponseCommand<Antecedent>? _actual;
    private readonly CreateAntecedentHandlerDecorator _decorator;

    public CreateAntecedentResponseStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var validator = fixture.Services.GetRequiredService<IValidator<Antecedent>>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();
        _antecedentService = fixture.Services.GetRequiredService<IEntityService<Antecedent>>();
        
        _sut = new(_uowFake);
        _decorator = new(validator, logger);            
    }

    [Given("An antecedent object with an empty name and description")]
    public void GivenAnAntecedentObjectWithAnEmptyNameAndDescription() =>
        _requestFake = CreateAntecedentResponseCommand.Create(
            string.Empty, string.Empty);

    [Given("antecedent service returns null")]
    public void GivenAntecedentServiceReturnsNull() =>
        A.CallTo(() => _antecedentService.GetByName(_requestFake!.Value.Name, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(default(Antecedent)));


    [When("executing the CreateAntecedentResponse handler")]
    public async Task WhenExecutingTheCreateAntecedentResponseHandler() =>
        _actual = await _decorator.Handle(
            _requestFake!,
            async (x, y) => await _sut.Handle(x, CancellationToken.None),
            CancellationToken.None);

    [Then("response should contain {int} error objects in array")]
    public void ThenResponseShouldContainErrorObjectsInArray(int expected)=>
        _actual?.Errors.Count.ShouldBe(expected, 
            string.Join(", ", _actual?.Errors.Select(e => e.Message) ?? []));

    [Given("an antecedent object with name: (\\w+) and description: (\\w+)")]
    public void GivenAnAntecedentObjectWithNameJoseAndDescriptionTest(
        string name,
        string description) =>
        _requestFake = CreateAntecedentResponseCommand.Create(
            name, description);

    [Given("the database is down")]
    public void GivenTheDatabaseIsDown() =>
        A.CallTo(() => _uowFake.Antecedents).Throws<InvalidOperationException>();

    [Given("SaveChanges returns {int} row affected")]
    public void GivenSaveChangesReturnsRowAffected(int rowsAffected) =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(Task.FromResult(rowsAffected));


    [Then("response should be true")]
    public void ThenResponseShouldBeTrue()
    {
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .MustHaveHappenedOnceExactly();

        _actual?.Entity.ShouldNotBeNull();
    }


}
