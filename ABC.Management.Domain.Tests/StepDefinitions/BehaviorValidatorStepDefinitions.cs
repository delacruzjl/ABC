using ABC.Management.Domain.Entities;

namespace ABC.Management.Domain.Tests.StepDefinitions;

[Binding]
public class BehaviorValidatorStepDefinitions : IClassFixture<StartupFixture>
{
    private Behavior? _behavior;
    private readonly IEntityService<Behavior> _service;
    private BehaviorValidator _sut;
    private ValidationResult? _actual;

    public BehaviorValidatorStepDefinitions(StartupFixture fixture)
    {
        _service = fixture.Services
            .GetRequiredService<IEntityService<Behavior>>();
        _sut = new(_service);
    }

    [Given("A Behavior with Id {string}, and attributes  {string} and {string}")]
    public void GivenABehaviorWithIdAndAttributesAnd(string p0, string p1, string p2)
    {
        _behavior = new(Guid.Parse(p0), p1, p2);
    }

    [Given("behavior name already exists")]
    public void GivenBehaviorNameAlreadyExists()
    {
        A.CallTo(() => _service.GetByName(_behavior!.Name, A<CancellationToken>.Ignored))
            .Returns(new Behavior(Guid.NewGuid()));
    }


    [When("validating the behavior")]
    public async Task WhenValidatingTheBehavior() =>
        _actual = await _sut.ValidateAsync(_behavior!);

    [Then("Should fail behavior validation")]
    public void ThenShouldFailBehaviorValidation()
    {
        _actual!.IsValid.ShouldBeFalse();
        A.CallTo(() => _service.GetByName(_behavior!.Name, A<CancellationToken>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

}
