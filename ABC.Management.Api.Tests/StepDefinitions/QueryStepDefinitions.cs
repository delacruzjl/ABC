using ABC.PostGreSQL;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace ABC.Management.Api.Tests.StepDefinitions
{
    [Binding]
    public class QueryStepDefinitions 
    {
        private readonly IUnitOfWork _uowFake;
        public QueryStepDefinitions(StartupFixture fixture) =>
            _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();

        [Given("a query request for antecedents")]
        public async Task GivenAQueryRequestForAntecedents() =>
            _ = await Types.Query.GetAntecedents(_uowFake, CancellationToken.None);

        [Then("the antecedents from the unit of work should be executed")]
        public void ThenTheAntecedentsFromTheUnitOfWorkShouldBeExecuted() =>
            A.CallTo(() => _uowFake.Antecedents.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for behaviors")]
        public async Task GivenAQueryRequestForBehaviors() =>
            _ = await Types.Query.GetBehaviors(_uowFake, CancellationToken.None);

        [Then("the hehaviors from the unit of work should be execute")]
        public void ThenTheHehaviorsFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Behaviors.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for consequences")]
        public async Task GivenAQueryRequestForConsequences() =>
            _ = await Types.Query.GetConsequences(_uowFake, CancellationToken.None);

        [Then("the consequences from the unit of work should be execute")]
        public void ThenTheConsequencesFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Consequences.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for children")]
        public async Task GivenAQueryRequestForChildren() =>
            _ = await Types.Query.GetChildren(_uowFake, CancellationToken.None);

        [Then("the children from the unit of work should be execute")]
        public void ThenTheChildrenFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Children.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }
}
