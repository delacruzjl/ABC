using Microsoft.Playwright;
using Reqnroll;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class AntecedentsCrudStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public AntecedentsCrudStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _scenarioContext.Get<IPage>();

    [When(@"I fill in the antecedent form with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenIFillInTheAntecedentForm(string name, string description)
    {
        await Page.GetByLabel("Antecedent Name").FillAsync(name);
        await Page.GetByLabel("Antecedent Description").FillAsync(description);
    }

    [When(@"I create an antecedent with name ""(.*)"" and description ""(.*)""")]
    public async Task WhenICreateAnAntecedent(string name, string description)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add Antecedent" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByLabel("Antecedent Name").FillAsync(name);
        await Page.GetByLabel("Antecedent Description").FillAsync(description);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I delete the antecedent ""(.*)""")]
    public async Task WhenIDeleteTheAntecedent(string name)
    {
        var row = Page.Locator("li").Filter(new() { HasText = name });
        await row.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        // Confirm deletion in the dialog
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).Last.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
