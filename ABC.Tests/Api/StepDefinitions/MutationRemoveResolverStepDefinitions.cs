using ABC.Management.Api.Commands;
using ABC.Management.Api.Types;
using ABC.SharedEntityFramework;
using FakeItEasy;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Shouldly;
using ABC.Tests.Fixtures;
using System.Security.Claims;

namespace ABC.Tests.Api.StepDefinitions;

[Binding]
public class MutationRemoveResolverStepDefinitions
{
    private readonly IMediator _mediatorFake;
    private readonly IResolverContext _resolverContext;
    private readonly IUnitOfWork _uowFake;
    private readonly ClaimsPrincipal _adminPrincipal;

    private Guid _existingGuid;
    private bool _actual = false;

    public MutationRemoveResolverStepDefinitions(ApiStartupFixture fixture)
    {
        _mediatorFake = fixture.Services.GetRequiredService<IMediator>();
        _resolverContext = fixture.Services.GetRequiredService<IResolverContext>();
        _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();
        A.CallTo(() => _resolverContext.HasErrors).Returns(false);

        _adminPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, Guid.CreateVersion7().ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        ], "test"));
    }

    [Given("I have a valid RemoveAntecedentCommand request")]
    public void GivenIHaveAValidRemoveAntecedentCommandRequest() =>
        _existingGuid = Guid.CreateVersion7();

    [When("I send the request to delete the antecedent")]
    public async Task WhenISendTheRequestToDeleteTheAntecedent() =>
        _actual = await Antecedents.RemoveAntecedent(
            _mediatorFake,
            _existingGuid,
            _resolverContext,
            CancellationToken.None);


    [Then("I should send a request to the RemoveAntecedentCommand handler")]
    public void ThenIShouldSendARequestToTheRemoveAntecedentCommandHandler()
    {
        A.CallTo(() => _mediatorFake.Send(
            A<RemoveAntecedentResponseCommand>.That.Matches(a => a.Entity.Id.Equals(_existingGuid)),
            A<CancellationToken>._))
        .MustHaveHappenedOnceExactly();

        _actual.ShouldBeTrue();
    }

    [Given("I have a valid RemoveBehaviorCommand request")]
    public void GivenIHaveAValidRemoveBehaviorCommandRequest() =>
        _existingGuid = Guid.CreateVersion7();

    [When("I send the request to delete the behavior")]
    public async Task WhenISendTheRequestToDeleteTheBehavior() =>
        _actual = await Behaviors.RemoveBehavior(
            _mediatorFake,
            _existingGuid,
            _resolverContext,
            CancellationToken.None);

    [Then("I should send a request to the RemoveBehaviorCommand handler")]
    public void ThenIShouldSendARequestToTheRemoveBehaviorCommandHandler()
    {
        A.CallTo(() => _mediatorFake.Send(
           A<RemoveBehaviorResponseCommand>.That.Matches(a => a.Entity.Id.Equals(_existingGuid)),
           A<CancellationToken>._))
       .MustHaveHappenedOnceExactly();

        _actual.ShouldBeTrue();
    }

    [Given("I have a valid RemoveConsequenceCommand request")]
    public void GivenIHaveAValidRemoveConsequenceCommandRequest() =>
        _existingGuid = Guid.CreateVersion7();

    [When("I send the request to delete the consequence")]
    public async Task WhenISendTheRequestToDeleteTheConsequence() =>
        _actual = await Consequences.RemoveConsequence(
            _mediatorFake,
            _existingGuid,
            _resolverContext,
            CancellationToken.None);

    [Then("I should send a request to the RemoveConsequenceCommand handler")]
    public void ThenIShouldSendARequestToTheRemoveConsequenceCommandHandler()
    {
        A.CallTo(() => _mediatorFake.Send(
           A<RemoveConsequenceResponseCommand>.That.Matches(a => a.Entity.Id.Equals(_existingGuid)),
           A<CancellationToken>._))
       .MustHaveHappenedOnceExactly();

        _actual.ShouldBeTrue();
    }

    [Given("I have a valid RemoveChildCommand request")]
    public void GivenIHaveAValidRemoveChildCommandRequest() =>
        _existingGuid = Guid.CreateVersion7();

    [When("I send the request to delete the child")]
    public async Task WhenISendTheRequestToDeleteTheChild() =>
        _actual = await Children.RemoveChild(
            _mediatorFake,
            _uowFake,
            _existingGuid,
            _adminPrincipal,
            _resolverContext,
            CancellationToken.None);

    [Then("I should send a request to the RemoveChildCommand handler")]
    public void ThenIShouldSendARequestToTheRemoveChildCommandHandler()
    {
        A.CallTo(() => _mediatorFake.Send(
           A<RemoveChildResponseCommand>.That.Matches(a => a.Entity.Id.Equals(_existingGuid)),
           A<CancellationToken>._))
       .MustHaveHappenedOnceExactly();

        _actual.ShouldBeTrue();
    }
}
