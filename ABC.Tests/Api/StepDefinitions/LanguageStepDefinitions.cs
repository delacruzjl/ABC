using ABC.Management.Api.Types;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using ABC.SharedKernel.Enums;
using Bogus;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Reqnroll;
using Shouldly;
using System.Security.Claims;

namespace ABC.Tests.Api.StepDefinitions;

[Binding]
public class LanguageStepDefinitions
{
    private readonly Faker _faker;
    private IDbContextFactory<ABCContext>? _dbContextFactory;
    private ClaimsPrincipal? _claimsPrincipal;
    private string? _userId;
    private string? _requestedLanguage;
    private bool _updatePreferredLanguageResult;
    private Exception? _exception;
    private Antecedent? _antecedent;
    private string? _spanishName;
    private string? _spanishDescription;
    private List<TranslatedEntity>? _translatedAntecedents;

    public LanguageStepDefinitions()
    {
        _faker = new Faker();
    }

    [Given("the current user has preferred language {string}")]
    public async Task GivenTheCurrentUserHasPreferredLanguage(string preferredLanguage)
    {
        _userId = Guid.CreateVersion7().ToString();
        _dbContextFactory = CreateDbContextFactory();
        _claimsPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, _userId)], "TestAuth"));

        var userName = _faker.Internet.UserName();
        var email = _faker.Internet.Email();

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.Users.Add(new ApplicationUser
        {
            Id = _userId,
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            PreferredLanguage = preferredLanguage
        });
        await dbContext.SaveChangesAsync();
    }

    [Given("the requested preferred language is {string}")]
    public void GivenTheRequestedPreferredLanguageIs(string language) =>
        _requestedLanguage = language;

    [Given("an antecedent exists with English values")]
    public async Task GivenAnAntecedentExistsWithEnglishValues()
    {
        _antecedent = new Antecedent(
            Guid.CreateVersion7(),
            _faker.Lorem.Word(),
            _faker.Lorem.Sentence());

        await using var dbContext = await _dbContextFactory!.CreateDbContextAsync();
        dbContext.Antecedents.Add(_antecedent);
        await dbContext.SaveChangesAsync();
    }

    [Given("a Spanish translation exists for the antecedent")]
    public async Task GivenASpanishTranslationExistsForTheAntecedent()
    {
        _spanishName = _faker.Lorem.Word();
        _spanishDescription = _faker.Lorem.Sentence();

        await using var dbContext = await _dbContextFactory!.CreateDbContextAsync();
        dbContext.EntityTranslations.Add(new EntityTranslation(
            TranslatableEntityType.Antecedent,
            _antecedent!.Id,
            "es",
            _spanishName,
            _spanishDescription));
        await dbContext.SaveChangesAsync();
    }

    [When("I update the preferred language")]
    public async Task WhenIUpdateThePreferredLanguage()
    {
        try
        {
            _updatePreferredLanguageResult = await ABC.Management.Api.Types.Language.UpdatePreferredLanguage(
                _requestedLanguage!,
                _claimsPrincipal!,
                _dbContextFactory!);
            _exception = null;
        }
        catch (Exception ex)
        {
            _exception = ex;
        }
    }

    [When("I request translated antecedents")]
    public async Task WhenIRequestTranslatedAntecedents()
    {
        using var unitOfWork = new UnitOfWork(_dbContextFactory!);
        var translationService = new TranslationService(_dbContextFactory!);
        _translatedAntecedents = await TranslatedEntities.GetTranslatedAntecedents(
            unitOfWork,
            translationService,
            _claimsPrincipal!,
            _dbContextFactory!,
            CancellationToken.None);
    }

    [Then("the preferred language update should succeed")]
    public void ThenThePreferredLanguageUpdateShouldSucceed()
    {
        _exception.ShouldBeNull();
        _updatePreferredLanguageResult.ShouldBeTrue();
    }

    [Then("the persisted preferred language should be {string}")]
    public async Task ThenThePersistedPreferredLanguageShouldBe(string expectedLanguage)
    {
        await using var dbContext = await _dbContextFactory!.CreateDbContextAsync();
        var user = await dbContext.Users.SingleAsync(user => user.Id == _userId);
        user.PreferredLanguage.ShouldBe(expectedLanguage);
    }

    [Then("the preferred language update should fail with unsupported language")]
    public void ThenThePreferredLanguageUpdateShouldFailWithUnsupportedLanguage()
    {
        _exception.ShouldNotBeNull();
        _exception.ShouldBeOfType<GraphQLException>();
        _exception.Message.ShouldContain("Unsupported language");
    }

    [Then("the antecedent should be returned in Spanish")]
    public void ThenTheAntecedentShouldBeReturnedInSpanish()
    {
        var translatedAntecedent = _translatedAntecedents!
            .Single(entity => entity.Id == _antecedent!.Id);

        translatedAntecedent.Name.ShouldBe(_spanishName);
        translatedAntecedent.Description.ShouldBe(_spanishDescription);
    }

    [Then("the antecedent should be returned in English")]
    public void ThenTheAntecedentShouldBeReturnedInEnglish()
    {
        var translatedAntecedent = _translatedAntecedents!
            .Single(entity => entity.Id == _antecedent!.Id);

        translatedAntecedent.Name.ShouldBe(_antecedent!.Name);
        translatedAntecedent.Description.ShouldBe(_antecedent.Description);
    }

    private IDbContextFactory<ABCContext> CreateDbContextFactory()
    {
        var options = new DbContextOptionsBuilder<ABCContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContextFactory(options);
    }

    private sealed class TestDbContextFactory(DbContextOptions<ABCContext> options) : IDbContextFactory<ABCContext>
    {
        public ABCContext CreateDbContext() => new(options);

        public Task<ABCContext> CreateDbContextAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(CreateDbContext());
    }
}
