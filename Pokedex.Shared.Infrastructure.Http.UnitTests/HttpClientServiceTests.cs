using System.Net;
using Microsoft.Extensions.Logging;
using Moq;

namespace Pokedex.Shared.Infrastructure.Http.UnitTests;

public class HttpClientServiceTests
{
    private HttpClient CreateHttpClient(HttpResponseMessage responseMessage)
    {
        var handler = new StubHttpMessageHandler(responseMessage);
        return new HttpClient(handler);
    }

    private ILogger<HttpClientService> CreateLogger()
    {
        return Mock.Of<ILogger<HttpClientService>>();
    }

    public class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public StubHttpMessageHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }

    public class ThrowingHttpMessageHandler : HttpMessageHandler
    {
        private readonly Exception _exception;
        public ThrowingHttpMessageHandler(Exception exception) => _exception = exception;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw _exception;
        }
    }

    private record TestResponse(string Name);

    [Fact]
    public async Task GetAsync_ReturnsDeserializedObject_OnSuccess()
    {
        // Arrange
        var json = "{\"name\":\"Pikachu\"}";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };
        var client = CreateHttpClient(response);
        var service = new HttpClientService(client, CreateLogger());

        // Act
        var result = await service.GetAsync<TestResponse>("https://test");

        // Assert
        Assert.True(result.IsT0);
        Assert.Equal("Pikachu", result.AsT0.Name);
    }

    [Fact]
    public async Task GetAsync_ReturnsHttpError_OnNonSuccessStatus()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("Not found")
        };
        var client = CreateHttpClient(response);
        var service = new HttpClientService(client, CreateLogger());

        var result = await service.GetAsync<TestResponse>("https://test");

        Assert.True(result.IsT1);
        Assert.Equal(404, result.AsT1.StatusCode);
    }

    [Fact]
    public async Task GetAsync_ReturnsHttpError_OnEmptyBody()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("")
        };
        var client = CreateHttpClient(response);
        var service = new HttpClientService(client, CreateLogger());

        var result = await service.GetAsync<TestResponse>("https://test");

        Assert.True(result.IsT1);
        Assert.Contains("Response was empty", result.AsT1.Message);
    }

    [Fact]
    public async Task GetAsync_ReturnsHttpError_OnJsonException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{invalid json}")
        };
        var client = CreateHttpClient(response);
        var loggerMock = new Mock<ILogger<HttpClientService>>();
        var service = new HttpClientService(client, loggerMock.Object);

        var result = await service.GetAsync<TestResponse>("https://test");

        Assert.True(result.IsT1);
        Assert.Contains("Failed to deserialize", result.AsT1.Message);
        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetAsync_ReturnsHttpError_OnHttpRequestException()
    {
        var handler = new ThrowingHttpMessageHandler(new HttpRequestException("boom"));
        var client = new HttpClient(handler);
        var service = new HttpClientService(client, CreateLogger());

        var result = await service.GetAsync<TestResponse>("https://test");

        Assert.True(result.IsT1);
        Assert.Contains("HTTP request failed", result.AsT1.Message);
    }

    [Fact]
    public async Task GetAsync_ReturnsHttpError_OnTimeout()
    {
        var ex = new TaskCanceledException("timeout", new TimeoutException());
        var handler = new ThrowingHttpMessageHandler(ex);
        var client = new HttpClient(handler);
        var service = new HttpClientService(client, CreateLogger());

        var result = await service.GetAsync<TestResponse>("https://test");

        Assert.True(result.IsT1);
        Assert.Contains("timed out", result.AsT1.Message);
    }
}
