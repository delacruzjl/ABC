using Microsoft.Playwright;
using Reqnroll;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class ChildrenCrudStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public ChildrenCrudStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IPage Page => _scenarioContext.Get<IPage>();

    [When(@"I fill in the child form with first name ""(.*)"" and last name ""(.*)"" and birth year ""(.*)""")]
    public async Task WhenIFillInTheChildForm(string firstName, string lastName, string birthYear)
    {
        await Page.GetByLabel("First Name").FillAsync(firstName);
        await Page.GetByLabel("Last Name").FillAsync(lastName);
        await Page.GetByLabel("Birth Year").FillAsync(birthYear);

        // Admin users must select an assigned user
        var userSelect = Page.GetByLabel("Assigned User");
        if (await userSelect.IsVisibleAsync())
        {
            // Select the first available user option
            var options = userSelect.Locator("option");
            var count = await options.CountAsync();
            if (count > 1)
                await userSelect.SelectOptionAsync(new SelectOptionValue { Index = 1 });
        }
    }

    [When(@"I create a child with first name ""(.*)"" and last name ""(.*)"" and birth year ""(.*)""")]
    public async Task WhenICreateAChild(string firstName, string lastName, string birthYear)
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Add Child" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByLabel("First Name").FillAsync(firstName);
        await Page.GetByLabel("Last Name").FillAsync(lastName);
        await Page.GetByLabel("Birth Year").FillAsync(birthYear);

        var userSelect = Page.GetByLabel("Assigned User");
        if (await userSelect.IsVisibleAsync())
        {
            var options = userSelect.Locator("option");
            var count = await options.CountAsync();
            if (count > 1)
                await userSelect.SelectOptionAsync(new SelectOptionValue { Index = 1 });
        }

        await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I click the edit button for child ""(.*)""")]
    public async Task WhenIClickTheEditButtonForChild(string firstName)
    {
        var card = Page.Locator("div").Filter(new() { HasText = firstName }).First;
        await card.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I update the child first name to ""(.*)""")]
    public async Task WhenIUpdateTheChildFirstName(string firstName)
    {
        var input = Page.GetByLabel("First Name");
        await input.ClearAsync();
        await input.FillAsync(firstName);
    }

    [When(@"I delete the child ""(.*)""")]
    public async Task WhenIDeleteTheChild(string firstName)
    {
        var card = Page.Locator("div").Filter(new() { HasText = firstName }).First;
        await card.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
        // Confirm deletion in the dialog
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
