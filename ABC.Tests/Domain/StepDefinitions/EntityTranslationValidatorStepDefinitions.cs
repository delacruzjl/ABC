using ABC.Management.Domain.Entities;
using ABC.Management.Domain.Validators;
using ABC.SharedKernel.Enums;
using Bogus;
using Reqnroll;
using Shouldly;

namespace ABC.Tests.Domain.StepDefinitions;

[Binding]
public class EntityTranslationValidatorStepDefinitions
{
    private readonly Faker _faker;
    private readonly EntityTranslationValidator _sut;
    private EntityTranslation? _entityTranslation;
    private FluentValidation.Results.ValidationResult? _actual;

    public EntityTranslationValidatorStepDefinitions()
    {
        _faker = new Faker();
        _sut = new EntityTranslationValidator();
    }

    [Given("A valid entity translation")]
    public void GivenAValidEntityTranslation() =>
        _entityTranslation = CreateEntityTranslation();

    [Given("An entity translation with an empty language")]
    public void GivenAnEntityTranslationWithAnEmptyLanguage() =>
        _entityTranslation = CreateEntityTranslation(language: string.Empty);

    [Given("An entity translation with an empty name")]
    public void GivenAnEntityTranslationWithAnEmptyName() =>
        _entityTranslation = CreateEntityTranslation(name: string.Empty);

    [Given("An entity translation with an empty description")]
    public void GivenAnEntityTranslationWithAnEmptyDescription() =>
        _entityTranslation = CreateEntityTranslation(description: string.Empty);

    [Given("An entity translation with an empty entity id")]
    public void GivenAnEntityTranslationWithAnEmptyEntityId() =>
        _entityTranslation = CreateEntityTranslation(entityId: Guid.Empty);

    [When("validating the entity translation")]
    public async Task WhenValidatingTheEntityTranslation() =>
        _actual = await _sut.ValidateAsync(_entityTranslation!);

    [Then("entity translation validation should succeed")]
    public void ThenEntityTranslationValidationShouldSucceed() =>
        _actual!.IsValid.ShouldBeTrue();

    [Then("entity translation validation should fail for {string}")]
    public void ThenEntityTranslationValidationShouldFailFor(string propertyName)
    {
        _actual!.IsValid.ShouldBeFalse();
        _actual.Errors.Select(error => error.PropertyName).ShouldContain(propertyName);
    }

    private EntityTranslation CreateEntityTranslation(
        Guid? entityId = null,
        string? language = null,
        string? name = null,
        string? description = null) =>
        new(
            TranslatableEntityType.Antecedent,
            entityId ?? Guid.CreateVersion7(),
            language ?? "es",
            name ?? _faker.Lorem.Word(),
            description ?? _faker.Lorem.Sentence());
}
