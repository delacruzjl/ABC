using Reqnroll;
using System.Threading.Tasks;

namespace ABC.PostgreSQL.Tests.Hooks;

[Binding]
internal class TestRunHook
{
    [BeforeTestRun]
    public static async Task BeforeFeature()
    {
        // Initialize the database or any other setup needed for the feature
        await StartupFixture.Instance.InitializeAsync();
    }
    [AfterTestRun]
    public static async Task AfterFeature()
    {
        // Clean up the database or any other teardown needed for the feature
        await StartupFixture.Instance.DisposeAsync();
    }
}
