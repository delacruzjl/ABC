using Microsoft.Playwright;
using Reqnroll;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class ConsequencesCrudStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public ConsequencesCrudStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _scenarioContext.Get<IPage>();

    [When(@"I fill in the consequence form with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenIFillInTheConsequenceForm(string name, string description)
    {
        await Page.GetByLabel("Consequence Name").FillAsync(name);
        await Page.GetByLabel("Consequence Description").FillAsync(description);
    }

    [When(@"I create a consequence with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenICreateAConsequence(string name, string description)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add Consequence" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByLabel("Consequence Name").FillAsync(name);
        await Page.GetByLabel("Consequence Description").FillAsync(description);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I delete the consequence ""(.*)""")]
    public async Task WhenIDeleteTheConsequence(string name)
    {
        var row = Page.Locator("li").Filter(new() { HasText = name });
        await row.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).Last.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
