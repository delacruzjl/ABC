using Reqnroll;
using System.Threading.Tasks;

namespace ABC.PostgreSQL.Tests.Hooks;

[Binding]
internal class FeatureHook
{
    [BeforeFeature]
    public static async Task BeforeFeature()
    {
        // Initialize the database or any other setup needed for the feature
        await StartupFixture.Instance.InitializeAsync();
    }
    [AfterFeature]
    public static async Task AfterFeature()
    {
        // Clean up the database or any other teardown needed for the feature
        await StartupFixture.Instance.DisposeAsync();
    }
}
