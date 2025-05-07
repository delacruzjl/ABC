using ABC.Management.Api.Commands;
using ABC.Management.Api.Decorators;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedKernel;
using Bogus.DataSets;
using FakeItEasy;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using Shouldly;
using System;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class CreateChildConditionStepDefinitions
{
    private readonly IEntityService<ChildCondition> _entityService;
    private readonly CreateChildConditionResponseHandler _sut;
    private readonly IUnitOfWork _uowFake;
    private CreateChildConditionResponseCommand? _requestFake;
    private BaseResponseCommand<ChildCondition>? _actual;
    private readonly CreateChildConditionHandlerDecorator _decorator;

    public CreateChildConditionStepDefinitions(StartupFixture fixture)
    {
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        var validator = fixture.Services.GetRequiredService<IValidator<ChildCondition>>();
        var logger = fixture.Services.GetRequiredService<ILogger<ErrorValidationDecorator>>();
        _entityService = fixture.Services.GetRequiredService<IEntityService<ChildCondition>>();

        _sut = new(_uowFake);
        _decorator = new(validator, logger);
    }

    [Given("An ChildCondition object with an empty name and description")]
    public void GivenAnChildConditionObjectWithAnEmptyNameAndDescription() =>
        _requestFake = CreateChildConditionResponseCommand.Create(string.Empty);

    [Given("ChildCondition service returns null")]
    public void GivenChildConditionServiceReturnsNull() =>
        A.CallTo(() => _entityService
            .GetByName(_requestFake!.Value.Name, A<CancellationToken>.Ignored))
        .Returns(Task.FromResult(default(ChildCondition)));

    [When("executing the CreateChildConditionResponse handler")]
    public async Task WhenExecutingTheCreateChildConditionResponseHandler() =>
        _actual = await _decorator.Handle(
            _requestFake!,
            async (x, y) => await _sut.Handle(x, CancellationToken.None),
            CancellationToken.None);

    [Then("child condition response should contain {int} error objects in array")]
    public void ThenChildConditionResponseShouldContainErrorObjectsInArray(int expected) =>
        _actual?.Errors.Count.ShouldBe(expected,
            string.Join(", ", _actual?.Errors.Select(e => e.Message) ?? []));

    [Given(@"an ChildCondition object with name: (\w+)")]
    public void GivenAnChildConditionObjectWithNameJoseAndDescriptionTest(string name) =>
        _requestFake = CreateChildConditionResponseCommand.Create(name);

    [Given("the child condition database is down")]
    public void GivenTheChildConditionDatabaseIsDown() =>
        A.CallTo(() => _uowFake.Antecedents).Throws<InvalidOperationException>();

    [Given("SaveChanges returns {int} child condition rows affected")]
    public void GivenSaveChangesReturnsChildConditionRowsAffected(int rowsAffected) =>
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .Returns(Task.FromResult(rowsAffected));

    [Then("child condition response should be true")]
    public void ThenChildConditionResponseShouldBeTrue()
    {
        A.CallTo(() => _uowFake.SaveChangesAsync())
            .MustHaveHappenedOnceExactly();

        _actual?.Entity.ShouldNotBeNull();
    }
}
