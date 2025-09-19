using DotNet.Testcontainers.Builders;
using Testcontainers.Redis;

namespace Pokedex.FunctionalTests.Configurations
{
    public class RedisTestContainer : IAsyncLifetime
    {
        private RedisContainer _container;
        public int Port { get; private set; }
        public string ConnectionString { get; private set; }
        public string Host { get; private set; }

        public async Task InitializeAsync()
        {
            _container = new RedisBuilder()
                .WithImage("redis:7.2-alpine")
                .WithPortBinding(6379, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(6379))
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .Build();

            await _container.StartAsync();
            
            Port = _container.GetMappedPublicPort(6379);
            Host = _container.Hostname;
            ConnectionString = $"{Host}:{Port},abortConnect=false,connectTimeout=5000,syncTimeout=1000";
        }

        public async Task DisposeAsync()
        {
            if (_container != null)
            {
                await _container.StopAsync();
                await _container.DisposeAsync();
            }
        }
    }
}