using ABC.Management.Api.Types;
using ABC.SharedEntityFramework;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System.Security.Claims;
using ABC.Tests.Fixtures;

namespace ABC.Tests.Api.StepDefinitions
{
    [Binding]
    public class QueryStepDefinitions
    {
        private readonly IUnitOfWork _uowFake;
        public QueryStepDefinitions(ApiStartupFixture fixture) =>
            _uowFake = fixture.Services.GetRequiredService<IUnitOfWork>();

        [Given("a query request for antecedents")]
        public async Task GivenAQueryRequestForAntecedents() =>
            _ = await Antecedents.GetAntecedents(_uowFake, CancellationToken.None);

        [Then("the antecedents from the unit of work should be executed")]
        public void ThenTheAntecedentsFromTheUnitOfWorkShouldBeExecuted() =>
            A.CallTo(() => _uowFake.Antecedents.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for behaviors")]
        public async Task GivenAQueryRequestForBehaviors() =>
            _ = await Behaviors.GetBehaviors(_uowFake, CancellationToken.None);

        [Then("the hehaviors from the unit of work should be execute")]
        public void ThenTheHehaviorsFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Behaviors.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for consequences")]
        public async Task GivenAQueryRequestForConsequences() =>
            _ = await Consequences.GetConsequences(_uowFake, CancellationToken.None);

        [Then("the consequences from the unit of work should be execute")]
        public void ThenTheConsequencesFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Consequences.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        [Given("a query request for children")]
        public async Task GivenAQueryRequestForChildren() =>
            _ = await Children.GetChildren(
                _uowFake,
                new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "Admin")])),
                CancellationToken.None);

        [Then("the children from the unit of work should be execute")]
        public void ThenTheChildrenFromTheUnitOfWorkShouldBeExecute() =>
            A.CallTo(() => _uowFake.Children.GetAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }
}
