using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Playwright;
using Reqnroll;
using ABC.Tests.E2E.Hooks;

namespace ABC.Tests.E2E.StepDefinitions;

[Binding]
public class AuthStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private static readonly HttpClient HttpClient = new()
    {
        DefaultRequestHeaders = { { "Accept", "application/json" } }
    };

    public AuthStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("I am logged in as an admin")]
    public async Task GivenIAmLoggedInAsAnAdmin()
    {
        var adminEmail = Environment.GetEnvironmentVariable("E2E_ADMIN_EMAIL") ?? "admin@abc.com";
        var adminPassword = Environment.GetEnvironmentVariable("E2E_ADMIN_PASSWORD") ?? "Admin123!";
        await LoginWithCredentials(adminEmail, adminPassword);
    }

    [Given(@"I am logged in as a user with email ""(.*)""")]
    public async Task GivenIAmLoggedInAsAUserWithEmail(string email)
    {
        await LoginWithDevExternalLogin(email);
    }

    private async Task LoginWithCredentials(string email, string password)
    {
        var graphqlUrl = GetGraphqlUrl();

        var mutation = new
        {
            query = @"mutation Login($email: String!, $password: String!) {
                login(email: $email, password: $password) {
                    token
                    email
                    roles
                }
            }",
            variables = new { email, password }
        };

        var token = await SendGraphqlMutation(graphqlUrl, mutation, "login");
        await SetTokenAndNavigate(token);
    }

    private async Task LoginWithDevExternalLogin(string email)
    {
        var graphqlUrl = GetGraphqlUrl();

        var mutation = new
        {
            query = @"mutation DevExternalLogin($provider: ExternalAuthProvider!, $email: String!) {
                devExternalLogin(provider: $provider, email: $email) {
                    token
                    email
                    roles
                }
            }",
            variables = new { provider = "GOOGLE", email }
        };

        var token = await SendGraphqlMutation(graphqlUrl, mutation, "devExternalLogin");
        await SetTokenAndNavigate(token);
    }

    private static string GetGraphqlUrl()
    {
        // Prefer direct API URL if available (bypasses webpack proxy for reliability)
        var apiUrl = Environment.GetEnvironmentVariable("E2E_API_URL");
        if (!string.IsNullOrEmpty(apiUrl))
            return $"{apiUrl}/graphql";

        return $"{PlaywrightHooks.BaseUrl}/api/graphql";
    }

    private static async Task<string> SendGraphqlMutation(string url, object mutation, string operationName)
    {
        var response = await HttpClient.PostAsJsonAsync(url, mutation);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"GraphQL request to {url} failed with status {response.StatusCode}: {body}");

        var json = JsonSerializer.Deserialize<JsonElement>(body);

        if (json.TryGetProperty("errors", out var errors))
            throw new InvalidOperationException(
                $"GraphQL {operationName} returned errors: {errors}");

        return json.GetProperty("data")
            .GetProperty(operationName)
            .GetProperty("token")
            .GetString()!;
    }

    private async Task SetTokenAndNavigate(string token)
    {
        var page = _scenarioContext.Get<IPage>();

        await page.GotoAsync($"{PlaywrightHooks.BaseUrl}/login");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await page.EvaluateAsync($"localStorage.setItem('abc_token', '{token}')");

        await page.GotoAsync(PlaywrightHooks.BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}
