using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedKernel;
using Bogus.DataSets;
using FakeItEasy;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;
using System.Collections.Generic;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class CreateChildStepDefinitions
{
    private readonly IEntityService<Child> _entityService;
    private readonly CreateChildResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private List<CreateChildResponseCommand> _requestFakes;
    private List<BaseResponseCommand<Child>> _actual;
    private readonly CreateChildHandlerDecorator _decorator;

    public CreateChildStepDefinitions(StartupFixture fixture)
    {
        _requestFakes = new();
        _actual = new();
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var validator = fixture.Services.GetRequiredService<IValidator<Child>>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();
        _entityService = fixture.Services.GetRequiredService<IEntityService<Child>>();

        _sut = new(_uowFake);
        _decorator = new(validator, logger);
    }

    [Given("the following Child data:")]
    public void GivenTheFollowingChildData(DataTable dataTable) =>
        _requestFakes.AddRange(dataTable.Rows.Select(row =>
            CreateChildResponseCommand.Create(
                row["LastName"],
                row["FirstName"],
                int.TryParse(row["BirthYear"], out var result) ? result : 0,
                row["Conditions"].Split(',')
                    .Select(str => new ChildCondition(str)))));

    [Given("calls to the Child service by name returns null")]
    public void GivenCallsToTheChildServiceByNameReturnsNull() =>
        A.CallTo(() => _entityService.GetByName(A<string>.Ignored, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(default(Child)));

    [Given("the Child should be saved in the database")]
    public void GivenTheChildShouldBeSavedInTheDatabase() =>
        A.CallTo(() =>
           _uowFake.SaveChangesAsync())
        .Returns(Task.FromResult(1));

    [When("I send a request to Child mutation")]
    public async Task WhenISendARequestToChildMutation()
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

    [Then("the Child should call the handler the same amount of times as the data I sent")]
    public void ThenTheChildShouldCallTheHandlerTheSameAmountOfTimesAsTheDataISent() =>
        A.CallTo(() =>
            _uowFake.Children.AddAsync(A<Child>.Ignored, A<CancellationToken>.Ignored))
        .MustHaveHappened(_requestFakes.Count, Times.Exactly);

    [Then("Child response should contain {int} error objects in array")]
    public void ThenChildResponseShouldContainErrorObjectsInArray(int errorCount) =>
        _actual.ShouldAllBe(x => x.Errors.Count == errorCount,
            string.Join(", ", _actual.SelectMany(e => e.Errors).Select(e => e.Message)));


    [Given(@"a Child object with name: (\w+) and description: (\w+)")]
    public void GivenAChildObjectWithNameJoseAndDescriptionTest(
        string lastName,
        string firstName) =>
        CreateChildResponseCommand.Create(lastName, firstName, 2020, []);

    [Given("the SaveChanges method does not affect any Child rows")]
    public void GivenTheSaveChangesMethodDoesNotAffectAnyChildRows() =>
        A.CallTo(() =>
           _uowFake.SaveChangesAsync())
        .Returns(Task.FromResult(0));
}
