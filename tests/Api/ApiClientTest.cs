using System.Net;
using System.Reflection;
using Moq;
using Moq.Protected;
using PassRegulaParser.Api;
using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Api;

public class ApiClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly ApiClient _apiClient;

    public ApiClientTests()
    {
        _mockHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5000/")
        };
        _apiClient = new ApiClient();
        // Replace the default HttpClient with our mocked one
        typeof(ApiClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(_apiClient, _httpClient);
    }

    [Fact]
    public async Task SendPassportDataAsync_Success_ReturnsTrue()
    {
        var passportData = new PassportData
        {
            FullName = "John Doe",
            Serial = "1234",
            Number = "567890"
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.True(result);
    }

    [Fact]
    public async Task SendPassportDataAsync_Failure_ReturnsFalse()
    {
        var passportData = new PassportData
        {
            FullName = "John Doe",
            Serial = "1234",
            Number = "567890"
        };

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"error\":\"Invalid data\"}")
        };
        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
    }

    [Fact]
    public async Task SendPassportDataAsync_HttpRequestException_Throws()
    {
        var passportData = new PassportData
        {
            FullName = "John Doe",
            Serial = "1234",
            Number = "567890"
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        await Assert.ThrowsAsync<HttpRequestException>(
            () => _apiClient.SendPassportDataAsync(passportData));
    }

    [Fact]
    public async Task SendPassportDataAsync_TaskCanceledException_Throws()
    {
        var passportData = new PassportData
        {
            FullName = "John Doe",
            Serial = "1234",
            Number = "567890"
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Request timeout"));

        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _apiClient.SendPassportDataAsync(passportData));
    }

    [Fact]
    public async Task SendPassportDataAsync_UnexpectedException_Throws()
    {
        var passportData = new PassportData
        {
            FullName = "John Doe",
            Serial = "1234",
            Number = "567890"
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("Unexpected error"));

        await Assert.ThrowsAsync<Exception>(
            () => _apiClient.SendPassportDataAsync(passportData));
    }
}
