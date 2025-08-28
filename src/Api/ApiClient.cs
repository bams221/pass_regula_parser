using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;
using PassRegulaParser.Ui.Services;

namespace PassRegulaParser.Api;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IMessageBoxService _messageBoxService;
    private const string ApiUrl = "http://localhost:5000/add_passport";

    public ApiClient() : this(new HttpClient(), new MessageBoxService())
    {
    }

    internal ApiClient(HttpClient httpClient, IMessageBoxService messageBoxService)
    {
        _httpClient = httpClient;
        _messageBoxService = messageBoxService;
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> SendPassportDataAsync(PassportData passportData)
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(passportData, options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiUrl, content);

            Console.WriteLine($"Data sent. Response status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Ошибка сервера: {response.StatusCode}\nДетали: {errorContent}";

                _messageBoxService.ShowError(errorMessage, "Ошибка отправки данных");
                return false;
            }
            return true;
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Ошибка HTTP запроса: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
            }

            _messageBoxService.ShowError(errorMessage, "Ошибка сети");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            string errorMessage = $"Превышено время ожидания запроса: {ex.Message}";
            _messageBoxService.ShowWarning(errorMessage, "Таймаут запроса");
            return false;
        }
        catch (Exception ex)
        {
            string errorMessage = $"Неожиданная ошибка: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
            }

            _messageBoxService.ShowError(errorMessage, "Неизвестная ошибка");
            return false;
        }
    }
}