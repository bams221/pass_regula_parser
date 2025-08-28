using System.Net;
using System.Reflection;
using Moq;
using Moq.Protected;
using PassRegulaParser.Api;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Api;

public class ApiClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly ApiClient _apiClient;
    private readonly Mock<IMessageBoxService> _mockMessageBoxService;

    public ApiClientTests()
    {
        _mockHandler = new Mock<HttpMessageHandler>();
        _mockMessageBoxService = new Mock<IMessageBoxService>();

        _httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5000/")
        };

        _apiClient = new ApiClient();

        // Replace the default HttpClient with our mocked one
        typeof(ApiClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(_apiClient, _httpClient);

        // Inject mock message box service
        typeof(ApiClient)
            .GetField("_messageBoxService", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(_apiClient, _mockMessageBoxService.Object);
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
        _mockMessageBoxService.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendPassportDataAsync_Failure_ReturnsFalseAndShowsErrorMessage()
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
        _mockMessageBoxService.Verify(
            x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task SendPassportDataAsync_HttpRequestException_ReturnsFalseAndShowsErrorMessage()
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

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
        _mockMessageBoxService.Verify(
            x => x.ShowError(It.IsAny<string>(), "Ошибка сети"),
            Times.Once);
    }

    [Fact]
    public async Task SendPassportDataAsync_TaskCanceledException_ReturnsFalseAndShowsWarning()
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

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
        _mockMessageBoxService.Verify(
            x => x.ShowWarning(It.IsAny<string>(), "Таймаут запроса"),
            Times.Once);
    }

    [Fact]
    public async Task SendPassportDataAsync_UnexpectedException_ReturnsFalseAndShowsErrorMessage()
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

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
        _mockMessageBoxService.Verify(
            x => x.ShowError(It.IsAny<string>(), "Неизвестная ошибка"),
            Times.Once);
    }
}
