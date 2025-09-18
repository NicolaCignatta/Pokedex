namespace Pokedex.FunctionalTests.Configurations;
public class FunctionalTestFixture : IAsyncLifetime
{
    public ApiTestContainer ApiContainer { get; private set; }
    public RedisTestContainer RedisTestContainer { get; private set; }
    public HttpClient HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        RedisTestContainer = new RedisTestContainer();
        ApiContainer = new ApiTestContainer(RedisTestContainer);
        await RedisTestContainer.InitializeAsync();
        HttpClient = ApiContainer.CreateClient();
    }

    public async Task DisposeAsync()
    {
        HttpClient.Dispose();
        await ApiContainer.DisposeAsync();
        await RedisTestContainer.DisposeAsync();
    }

    [CollectionDefinition("ApiTestCollection")]
    public class ApiTestCollection : ICollectionFixture<FunctionalTestFixture>
    {
    }
}