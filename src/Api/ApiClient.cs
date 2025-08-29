using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PassRegulaParser.Config;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;
using PassRegulaParser.Ui.Services;

namespace PassRegulaParser.Api;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiErrorHandler _errorHandler;
    private readonly string _apiUrl;

    public ApiClient() : this(new HttpClient(), new MessageBoxService()) { }

    protected ApiClient(HttpClient httpClient, IMessageBoxService messageBoxService)
    {
        _httpClient = httpClient;
        _errorHandler = new ApiErrorHandler(messageBoxService);
        _apiUrl = AppConfiguration.Configuration["ApiSettings:ApiUrl"]
            ?? throw new InvalidOperationException("В конфигурационном файле appsettings.json не найден адрес API (ApiSettings:ApiUrl).");

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> SendPassportDataAsync(PassportData passportData)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(passportData, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);
            Console.WriteLine($"Данные отправлены. Статус ответа: {response.StatusCode}");

            var errorContent = await response.Content.ReadAsStringAsync();
            return _errorHandler.HandleError(response, errorContent);
        }
        catch (Exception ex)
        {
            _errorHandler.HandleException(ex);
            return false;
        }
    }
}
