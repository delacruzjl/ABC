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
public class CreateBehaviorStepDefinitions
{
    private readonly IEntityService<Behavior> _entityService;
    private readonly CreateBehaviorResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private List<CreateBehaviorResponseCommand> _requestFakes;
    private List<BaseResponseCommand<Behavior>> _actual;
    private readonly CreateBehaviorHandlerDecorator _decorator;

    public CreateBehaviorStepDefinitions(StartupFixture fixture)
    {
        _requestFakes = new();
        _actual = new();
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var validator = fixture.Services.GetRequiredService<IValidator<Behavior>>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();
        _entityService = fixture.Services.GetRequiredService<IEntityService<Behavior>>();

        _sut = new(_uowFake);
        _decorator = new(validator, logger);
    }

    [Given("the following behavior data:")]
    public void GivenTheFollowingBehaviorData(DataTable dataTable) =>
        _requestFakes.AddRange(dataTable.Rows.Select(row =>
            CreateBehaviorResponseCommand.Create(
                row["Name"],
                row["Description"])));

    [Given("calls to the behavior service by name returns null")]
    public void GivenCallsToTheBehaviorServiceByNameReturnsNull() =>
        A.CallTo(() => _entityService.GetByName(A<string>.Ignored, A<CancellationToken>.Ignored))
            .Returns(Task.FromResult(default(Behavior)));


    [When("I send a request to behavior mutation")]
    public async Task WhenISendAPOSTRequestToBehaviorMutation()
    {
        foreach(var r in _requestFakes)
        {
            var response = await _decorator.Handle(
                r,
                async (commandRequest, ct) => await _sut.Handle(commandRequest, ct),
                CancellationToken.None);

            _actual.Add(response);
        }
    }

    [Then("the response should call the handler the same amount of times as the data I sent")]
    public void ThenTheResponseBodyShouldContainTheCreatedBehaviorWithTheSameData() =>
         A.CallTo(() =>
            _uowFake.Behaviors.AddAsync(A<Behavior>.Ignored, A<CancellationToken>.Ignored))
        .MustHaveHappened(_requestFakes.Count, Times.Exactly);

    [Given("the behavior should be saved in the database")]
    public void GivenTheBehaviorShouldBeSavedInTheDatabase()
    {
        A.CallTo(() =>
           _uowFake.SaveChangesAsync())
        .Returns(Task.FromResult(1));
    }


    [Given(@"a behavior object with name: (\w+) and description: (\w+)")]
    public void GivenABehaviorObjectWithNameJoseAndDescriptionTest(
        string name,
        string description) =>
        CreateBehaviorResponseCommand.Create(name, description);

    [Then(@"behavior response should contain (\d+) error objects in array")]
    public void ThenThereShouldBeNoErrors(int errorCount) =>
        _actual.ShouldAllBe(x => x.Errors.Count == errorCount, 
            string.Join(", ", _actual.SelectMany(e => e.Errors).Select(e => e.Message)));


}
