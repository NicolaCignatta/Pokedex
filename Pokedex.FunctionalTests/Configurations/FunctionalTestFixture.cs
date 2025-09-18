namespace Pokedex.FunctionalTests.Configurations;
public class FunctionalTestFixture : IAsyncLifetime
{
    public ApiTestContainer ApiContainer { get; private set; }
    public HttpClient HttpClient { get; private set; }

    public Task InitializeAsync()
    {
        ApiContainer = new ApiTestContainer();
        HttpClient = ApiContainer.CreateClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        HttpClient.Dispose();
        await ApiContainer.DisposeAsync();
    }


    [CollectionDefinition("ApiTestCollection")]
    public class ApiTestCollection : ICollectionFixture<FunctionalTestFixture>
    {
    }
}