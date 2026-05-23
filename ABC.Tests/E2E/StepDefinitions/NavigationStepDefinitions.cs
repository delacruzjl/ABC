using Microsoft.Playwright;
using Reqnroll;
using ABC.Tests.E2E.Hooks;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class NavigationStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public NavigationStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _scenarioContext.Get<IPage>();

    [When(@"I navigate to ""(.*)""")]
    public async Task WhenINavigateTo(string path)
    {
        await Page.GotoAsync($"{PlaywrightHooks.BaseUrl}{path}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I click the ""(.*)"" nav button")]
    public async Task WhenIClickTheNavButton(string label)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = label }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I click the ""(.*)"" button")]
    public async Task WhenIClickTheButton(string label)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = label }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Then(@"I should see ""(.*)"" on the page")]
    public async Task ThenIShouldSeeOnThePage(string text)
    {
        var locator = Page.GetByText(text).First;
        await Assertions.Expect(locator).ToBeVisibleAsync();
    }

    [Then(@"I should not see ""(.*)"" on the page")]
    public async Task ThenIShouldNotSeeOnThePage(string text)
    {
        var locator = Page.GetByText(text, new() { Exact = true }).First;
        await Assertions.Expect(locator).Not.ToBeVisibleAsync();
    }

    [Then(@"I should be on the ""(.*)"" page")]
    public async Task ThenIShouldBeOnThePage(string path)
    {
        await Assertions.Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex($".*{System.Text.RegularExpressions.Regex.Escape(path)}"));
    }

    [Then(@"the page should contain heading ""(.*)""")]
    public async Task ThenThePageShouldContainHeading(string heading)
    {
        var locator = Page.GetByRole(AriaRole.Heading, new() { Name = heading }).First;
        await Assertions.Expect(locator).ToBeVisibleAsync();
    }
}
