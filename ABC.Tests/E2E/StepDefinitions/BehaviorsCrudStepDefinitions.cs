using Microsoft.Playwright;
using Reqnroll;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class BehaviorsCrudStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public BehaviorsCrudStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _scenarioContext.Get<IPage>();

    [When(@"I fill in the behavior form with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenIFillInTheBehaviorForm(string name, string description)
    {
        await Page.GetByLabel("Behavior Name").FillAsync(name);
        await Page.GetByLabel("Behavior Description").FillAsync(description);
    }

    [When(@"I create a behavior with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenICreateABehavior(string name, string description)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add Behavior" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByLabel("Behavior Name").FillAsync(name);
        await Page.GetByLabel("Behavior Description").FillAsync(description);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I delete the behavior ""(.*)""")]
    public async Task WhenIDeleteTheBehavior(string name)
    {
        var row = Page.Locator("li").Filter(new() { HasText = name });
        await row.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).Last.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
