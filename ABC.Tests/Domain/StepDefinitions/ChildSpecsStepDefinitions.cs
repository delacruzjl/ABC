using ABC.Management.Domain.Validators;
using FluentValidation.Results;
using ABC.Tests.Fixtures;
using ABC.SharedKernel;

namespace ABC.Tests.Domain.StepDefinitions;

[Binding]
public class ChildSpecsStepDefinitions : IClassFixture<DomainStartupFixture>
{
    private Child? _child;
    private ValidationResult? _validationResult;
    private IEntityService<ChildCondition> _entityService;
    private readonly ChildValidator _childValidator;

    public ChildSpecsStepDefinitions(DomainStartupFixture fixture)
    {
        _entityService = fixture.Services.GetRequiredService<IEntityService<ChildCondition>>();
        _childValidator = new(_entityService);

    }

    [Given("I create a Child entity")]
    public void GivenICreateAChildEntity() =>
        _child = new();

    [Given("Last Name is empty")]
    public void GivenLastNameIsEmpty() =>
        _child = new(Guid.NewGuid(), null!, "Fake Last", 13, []);

    [When("Validating child")]
    public async Task WhenValidatingChild() =>
        _validationResult = await _childValidator.ValidateAsync(_child!);

    [Then("validation should be false")]
    public void ThenValidationShouldBeFalse() =>
        _validationResult?.IsValid.ShouldBeFalse();

    [Given("Last Name is {string}")]
    public void GivenLastNameIs(string lastName) =>
        _child = new() { LastName = lastName };

    [Given("First name is {string}")]
    public void GivenFirstNameIs(string firstName) =>
        _child = new(_child!.Id)
        {
            FirstName = firstName,
            LastName = _child.LastName,
            BirthYear = _child.BirthYear
        };

    [Given("birth year is {int}")]
    public void GivenBirthYearIs(int birthYear) =>
        _child = new(_child!.Id)
        {
            FirstName = _child.FirstName,
            LastName = _child.LastName,
            BirthYear = birthYear
        };

    [Then("validation should be true")]
    public void ThenValidationShouldBeTrue() =>
        _validationResult!.IsValid.ShouldBeTrue();

    [Given("all conditions are found")]
    public void GivenAllConditionsAreFound() =>
        A.CallTo(() => _entityService.GetByName(A<string>.Ignored, A<CancellationToken>.Ignored))!
            .Returns(Task.FromResult(new ChildCondition(Guid.NewGuid(), "Fake Condition")));

    [Given("domain condition from the list is not found")]
    public void GivenConditionFromTheListIsNotFound() =>
        A.CallTo(() => _entityService.GetByName(A<string>.Ignored, A<CancellationToken>.Ignored))!
            .Returns(Task.FromResult(default(ChildCondition)));

    [Given("Child conditions contain: {string}")]
    public async Task GivenChildConditionsContain(string conditions) =>
        await _child!.SetChildConditions(_entityService, conditions.Split(","), CancellationToken.None);
}
