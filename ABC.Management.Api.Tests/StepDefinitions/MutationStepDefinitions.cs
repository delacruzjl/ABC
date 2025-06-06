using ABC.Management.Api.Commands;
using ABC.Management.Api.Types;
using ABC.Management.Domain.Entities;
using Bogus.DataSets;
using FakeItEasy;
using HotChocolate.Resolvers;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace ABC.Management.Api.Tests.StepDefinitions
{
    [Binding]
    public class MutationStepDefinitions
    {
        private readonly Lorem _lorem;
        private readonly Name _nameGenerator;
        private readonly IMediator _mediatorFake;
        private readonly IResolverContext _resolverContext;
        private CreateAntecedentResponseCommand? _antecedentCommand;
        private CreateBehaviorResponseCommand? _behaviorCommand;
        private CreateConsequenceResponseCommand? _consequenceCommand;
        private CreateChildResponseCommand? _childCommand;

        public MutationStepDefinitions(StartupFixture fixture)
        {
            _lorem = new Bogus.DataSets.Lorem(locale: "en");
            _nameGenerator = new Bogus.DataSets.Name();

            _mediatorFake = fixture.Services.GetRequiredService<IMediator>();
            _resolverContext = fixture.Services.GetRequiredService<IResolverContext>();
        }

        [Given("I have a valid CreateAntecedentCommand request")]
        public Task GivenIHaveAValidCreateAntecedentCommandRequest()
        {
            _antecedentCommand = CreateAntecedentResponseCommand.Create(
               _lorem.Word(),
                _lorem.Sentence());

            return Task.CompletedTask;
        }

        [When("I send the request to the antecedents API")]
        public async Task WhenISendTheRequestToTheAPI() =>
            await Antecedents.CreateAntecedentAsync(
                _mediatorFake,
                _antecedentCommand!.Value.Name,
                _antecedentCommand!.Value.Description,
                _resolverContext,
                CancellationToken.None);

        [Then("I should send a request to the CreateAntecedentCommand handler")]
        public void ThenIShouldSendARequestToTheCreateAntecedentCommandHandler() =>
            A.CallTo(() => _mediatorFake.Send(
                A<IRequest<BaseResponseCommand<Antecedent>>>._,
                A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("I have a valid CreateBehaviorCommand request")]
        public void GivenIHaveAValidCreateBehaviorCommandRequest()
        {
            _behaviorCommand = CreateBehaviorResponseCommand.Create(
                _lorem.Word(),
                _lorem.Sentence());
        }

        [Then("I should send a request to the CreateBehaviorCommand handler")]
        public void ThenIShouldSendARequestToTheCreateBehaviorCommandHandler() =>
            A.CallTo(() => _mediatorFake.Send(
                A<IRequest<BaseResponseCommand<Behavior>>>._,
                A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("I have a valid CreateConsequenceCommand request")]
        public void GivenIHaveAValidCreateConsequenceCommandRequest()
        {
            _consequenceCommand = CreateConsequenceResponseCommand.Create(
                _lorem.Word(),
                _lorem.Sentence());
        }

        [Then("I should send a request to the CreateConsequenceCommand handler")]
        public void ThenIShouldSendARequestToTheCreateConsequenceCommandHandler() =>
            A.CallTo(() => _mediatorFake.Send(
                A<IRequest<BaseResponseCommand<Consequence>>>._,
                A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("I have a valid CreateChildCommand request")]
        public void GivenIHaveAValidCreateChildCommandRequest()
        {
            _childCommand = CreateChildResponseCommand.Create(
                _nameGenerator.LastName(),
                _nameGenerator.FirstName(),
                10,
                 [_lorem.Word()]);
        }

        [Then("I should send a request to the CreateChildCommand handler")]
        public void ThenIShouldSendARequestToTheCreateChildCommandHandler() =>
            A.CallTo(() => _mediatorFake.Send(
                A<IRequest<BaseResponseCommand<Child>>>._,
                A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [When("I send the request to the behaviors API")]
        public async Task WhenISendTheRequestToTheBehaviorsAPI() =>
            await Behaviors.CreateBehavior(
                _mediatorFake,
                _behaviorCommand!.Value.Name,
                _behaviorCommand!.Value.Description,
                _resolverContext,
                CancellationToken.None);

        [When("I send the request to the consequences API")]
        public async Task WhenISendTheRequestToTheConsequencesAPI() =>
            await Consequences.CreateConsequence(
                _mediatorFake,
                _consequenceCommand!.Value.Name,
                _consequenceCommand!.Value.Description,
                _resolverContext,
                CancellationToken.None);

        [When("I send the request to the children API")]
        public async Task WhenISendTheRequestToTheChildrenAPI() =>
            await Children.CreateChild(
                _mediatorFake,
                _childCommand!.Value.LastName,
                _childCommand!.Value.FirstName,
                _childCommand.Value.BirthYear,
                _childCommand.Value.Conditions.Select(c => c.Name),
                _resolverContext,
                CancellationToken.None);

    }
}
