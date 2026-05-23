using ABC.Management.Domain.Validators;
using FluentValidation.Results;
using ABC.Tests.Fixtures;
namespace ABC.Management.Domain.Tests.StepDefinitions
{
    [Binding]
    public class AntecedentValidatorSpecsStepDefinitions : IClassFixture<DomainStartupFixture>
    {
        private Antecedent? _antecedent;
        private readonly IEntityService<Antecedent> _service;
        private AntecedentValidator _sut;
        private ValidationResult? _actual;

        public AntecedentValidatorSpecsStepDefinitions(DomainStartupFixture fixture)
        {
            _service = fixture.Services
                .GetRequiredService<IEntityService<Antecedent>>();
            _sut = new(_service);
        }


        [Given("An antecedent with empty name")]
        public void GivenAnAntecedentWithEmptyName()
        {
            _antecedent = new(Guid.CreateVersion7());
        }

        [When("validating")]
        public async Task WhenValidating() =>
            _actual = await _sut.ValidateAsync(_antecedent!);

        [Then("Should throw validation exception")]
        public void ThenShouldThrowValidationException()
        {
            _actual?.IsValid.ShouldBeFalse();
        }

        [Given("An antecedent with attributes {string} and empty description")]
        public void GivenAnAntecedentWithAttributesAnd(string name)
        {
            _antecedent = new(Guid.CreateVersion7(), name, null!);
        }

        [Given("Antecedent with the same name exists")]
        public void GivenAntecedentWithTheSameNameExists() =>
            A.CallTo(() => _service.GetByName(_antecedent!.Name, A<CancellationToken>.Ignored))
            .Returns(new Antecedent(Guid.CreateVersion7()));


        [Given("An antecedent with Id {string}, and attributes  {string} and {string}")]
        public void GivenAnAntecedentWithIdAndAttributesAnd(string id, string name, string description)
        {
            _antecedent = new(Guid.CreateVersion7(), name, description);
        }
    }
}
