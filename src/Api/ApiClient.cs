using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PassRegulaParser.Core.Interfaces;
using PassRegulaParser.Models;
using PassRegulaParser.Ui.Services;

namespace PassRegulaParser.Api;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IMessageBoxService _messageBoxService;
    private readonly string _apiUrl;

    public ApiClient() : this(new HttpClient(), new MessageBoxService())
    {
    }

    internal ApiClient(HttpClient httpClient, IMessageBoxService messageBoxService)
    {
        _httpClient = httpClient;
        _messageBoxService = messageBoxService;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _apiUrl = configuration["ApiSettings:ApiUrl"] ??
            throw new InvalidOperationException("В конфигурационном файле appsettings.json не найден адрес API (ApiSettings:ApiUrl).");

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

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Статус: {response.StatusCode}\n" +
                                      $"Подробности: {(!string.IsNullOrEmpty(errorContent) ? errorContent : "нет дополнительной информации")}";
                _messageBoxService.ShowError(errorMessage, "Ошибка при отправке данных");
                return false;
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Не удалось отправить данные на сервер.\n" +
                                  $"Проверьте подключение к интернету и повторите попытку.\n\n" +
                                  $"Детали ошибки: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
            }
            _messageBoxService.ShowError(errorMessage, "Ошибка сети");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            string errorMessage = $"Превышено время ожидания ответа от сервера.\n" +
                                  $"Попробуйте повторить попытку позже или проверьте стабильность соединения.\n\n" +
                                  $"Детали ошибки: {ex.Message}";
            _messageBoxService.ShowWarning(errorMessage, "Таймаут запроса");
            return false;
        }
        catch (JsonException ex)
        {
            string errorMessage = $"Ошибка при обработке данных паспорта.\n" +
                                  $"Проверьте корректность введённых данных и повторите попытку.\n\n" +
                                  $"Детали ошибки: {ex.Message}";
            _messageBoxService.ShowError(errorMessage, "Ошибка данных");
            return false;
        }
        catch (Exception ex)
        {
            string errorMessage = $"Произошла неожиданная ошибка при отправке данных.\n" +
                                  $"Попробуйте повторить попытку позже или обратитесь к администратору.\n\n" +
                                  $"Детали ошибки: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
            }
            _messageBoxService.ShowError(errorMessage, "Неизвестная ошибка");
            return false;
        }
    }
}
