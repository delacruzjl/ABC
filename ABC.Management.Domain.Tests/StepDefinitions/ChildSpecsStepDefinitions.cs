namespace ABC.Management.Domain.Tests.StepDefinitions;

[Binding]
public class ChildSpecsStepDefinitions : IClassFixture<StartupFixture>
{
    private Child? _child;
    private ValidationResult? _validationResult;
    private readonly ChildValidator _childValidator;

    public ChildSpecsStepDefinitions(StartupFixture fixture)
    {
        _childValidator = new();
    }

    [Given("I create a Child entity")]
    public void GivenICreateAChildEntity()
    {
        _child = new();
    }

    [Given("Last Name is empty")]
    public void GivenLastNameIsEmpty()
    {
        _child = new(Guid.NewGuid(), null!, "Fake Last", 13);
    }

    [When("Validating child")]
    public async Task WhenValidatingChild() =>
        _validationResult = await _childValidator.ValidateAsync(_child!);

    [Then("validation should be false")]
    public void ThenValidationShouldBeFalse()
    {
        _validationResult?.IsValid.Should().BeFalse();
    }

    [Given("Last Name is {string}")]
    public void GivenLastNameIs(string lastName)
    {
        _child = new() { LastName = lastName };
    }

    [Given("First name is {string}")]
    public void GivenFirstNameIs(string firstName)
    {
        _child = new()
        {
            Id = _child!.Id,
            FirstName = firstName,
            LastName = _child.LastName,
            Age = _child.Age
        };
    }


    [Given("Age is {int}")]
    public void GivenAgeIs(int age)
    {
        _child = new()
        {
            Id = _child!.Id,
            FirstName = _child.FirstName,
            LastName = _child.LastName,
            Age = age
        };
    }

    [Then("validation should be true")]
    public void ThenValidationShouldBeTrue()
    {
        _validationResult!.IsValid.Should().BeTrue();
    }
}
