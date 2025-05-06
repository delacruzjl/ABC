using ABC.Management.Api.Commands;
using ABC.Management.Api.Types;
using FakeItEasy;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Shouldly;

namespace ABC.Management.Api.Tests.StepDefinitions;

[Binding]
public class MutationRemoveResolverStepDefinitions
{
    private readonly IMediator _mediatorFake;
    private readonly IResolverContext _resolverContext;

    private Guid _existingGuid;
    private bool _actual = false;

    public MutationRemoveResolverStepDefinitions(StartupFixture fixture)
    {
        _mediatorFake = fixture.Services.GetRequiredService<IMediator>();
        _resolverContext = fixture.Services.GetRequiredService<IResolverContext>();
        A.CallTo(() => _resolverContext.HasErrors).Returns(false);
    }

    [Given("I have a valid RemoveAntecedentCommand request")]
    public void GivenIHaveAValidRemoveAntecedentCommandRequest() =>
        _existingGuid = Guid.NewGuid();

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
        _existingGuid = Guid.NewGuid();

    [When("I send the request to delete the behavior")]
    public async Task WhenISendTheRequestToDeleteTheBehavior() =>
        _actual = await Types.Behaviors.RemoveBehavior(
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
        _existingGuid = Guid.NewGuid();

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
        _existingGuid = Guid.NewGuid();

    [When("I send the request to delete the child")]
    public async Task WhenISendTheRequestToDeleteTheChild() =>
        _actual = await Children.RemoveChild(
            _mediatorFake,
            _existingGuid,
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
