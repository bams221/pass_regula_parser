using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using PassRegulaParser.Api;
using PassRegulaParser.Config;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;

namespace PassRegulaParser.Tests.Api;

public class ApiClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IMessageBoxService> _messageBoxServiceMock;
    private readonly ApiClient _apiClient;

    public ApiClientTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _messageBoxServiceMock = new Mock<IMessageBoxService>();

        AppConfiguration.Configuration["ApiSettings:ApiUrl"] = "https://test-api.com";

        _apiClient = new ApiClient(_httpClient, _messageBoxServiceMock.Object);
    }

    [Fact]
    public async Task SendPassportDataAsync_SuccessfulRequest_ReturnsTrue()
    {
        var passportData = new PassportData();
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.True(result);
    }

    [Fact]
    public async Task SendPassportDataAsync_ErrorResponseWithJson_ReturnsFalse()
    {
        var passportData = new PassportData();
        var errorResponse = new ErrorResponse { Error = "Invalid data" };
        var json = JsonSerializer.Serialize(errorResponse);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
    }

    [Fact]
    public async Task SendPassportDataAsync_ErrorResponseWithPlainText_ReturnsFalse()
    {
        var passportData = new PassportData();
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Internal Server Error", Encoding.UTF8, "text/plain")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
    }

    [Fact]
    public async Task SendPassportDataAsync_ExceptionThrown_ReturnsFalse()
    {
        var passportData = new PassportData();

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var result = await _apiClient.SendPassportDataAsync(passportData);

        Assert.False(result);
    }

    [Fact]
public void ApiClient_Constructor_MissingApiUrl_ThrowsException()
{
    AppConfiguration.Configuration["ApiSettings:ApiUrl"] = null;

    Assert.Throws<InvalidOperationException>(() => new ApiClient());
}

}
