using Microsoft.Playwright;
using Reqnroll;

namespace ABC.Tests.E2E.Hooks;

[Binding]
public class PlaywrightHooks
{
    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private static readonly SemaphoreSlim InitLock = new(1, 1);

    private readonly ScenarioContext _scenarioContext;

    public PlaywrightHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:4001";

    private static async Task EnsureInitializedAsync()
    {
        if (_browser != null) return;

        await InitLock.WaitAsync();
        try
        {
            if (_browser != null) return;

            _playwright = await Playwright.CreateAsync();

            var headless = !string.Equals(
                Environment.GetEnvironmentVariable("E2E_HEADED"), "true",
                StringComparison.OrdinalIgnoreCase);

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = headless
            });
        }
        finally
        {
            InitLock.Release();
        }
    }

    [BeforeScenario("e2e")]
    public async Task BeforeScenario()
    {
        await EnsureInitializedAsync();

        var context = await _browser!.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        var page = await context.NewPageAsync();

        _scenarioContext.Set(context);
        _scenarioContext.Set(page);
    }

    [AfterScenario("e2e")]
    public async Task AfterScenario()
    {
        if (_scenarioContext.TestError != null)
        {
            var page = _scenarioContext.Get<IPage>();
            var screenshotDir = Path.Combine(
                AppContext.BaseDirectory, "screenshots");
            Directory.CreateDirectory(screenshotDir);

            var fileName = $"{_scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = Path.Combine(screenshotDir, fileName),
                FullPage = true
            });
        }

        if (_scenarioContext.TryGetValue<IPage>(out var p))
            await p.CloseAsync();

        if (_scenarioContext.TryGetValue<IBrowserContext>(out var ctx))
            await ctx.CloseAsync();
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
