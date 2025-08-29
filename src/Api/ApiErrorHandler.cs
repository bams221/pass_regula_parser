using System.Net;
using System.Text.Json;
using PassRegulaParser.Core.Interfaces;

namespace PassRegulaParser.Api;

public class ApiErrorHandler(IMessageBoxService messageBoxService)
{
    private readonly IMessageBoxService _messageBoxService = messageBoxService;

    public bool HandleError(
        HttpResponseMessage response,
        string errorContent,
        Exception? exception = null)
    {
        if (response.IsSuccessStatusCode)
            return true;

        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            HandleValidationError(errorContent);
        }
        else
        {
            HandleServerErrorAsync(response.StatusCode, errorContent);
        }

        return false;
    }

    public void HandleException(Exception ex)
    {
        switch (ex)
        {
            case HttpRequestException httpEx:
                HandleHttpRequestErrorAsync(httpEx);
                break;
            case TaskCanceledException timeoutEx:
                HandleTimeoutErrorAsync(timeoutEx);
                break;
            case JsonException jsonEx:
                HandleJsonErrorAsync(jsonEx);
                break;
            default:
                HandleUnexpectedErrorAsync(ex);
                break;
        }
    }

    private void HandleValidationError(string errorContent)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(errorContent);
            var errors = doc.RootElement.GetProperty("detail").EnumerateArray();
            var errorMessages = new List<string>();
            foreach (var error in errors)
            {
                string msg = error.GetProperty("msg").GetString() ?? "";
                errorMessages.Add(msg);
            }
            string errorMessage = $"Ошибка валидации данных паспорта:\n\n" +
                                  $"{string.Join("\n", errorMessages)}\n\n" +
                                  $"Проверьте корректность введённых данных и повторите попытку.";
            _messageBoxService.ShowError(errorMessage, "Ошибка валидации данных");
        }
        catch (Exception)
        {
            string errorMessage = $"Ошибка валидации данных паспорта.\n" +
                                  $"Проверьте корректность введённых данных и повторите попытку.\n\n" +
                                  $"Детали ошибки: {(!string.IsNullOrEmpty(errorContent) ? errorContent : "нет дополнительной информации")}";
            _messageBoxService.ShowError(errorMessage, "Ошибка валидации данных");
        }
    }

    private void HandleServerErrorAsync(HttpStatusCode statusCode, string errorContent)
    {
        string errorMessage = $"Сервер вернул ошибку: {statusCode}\n" +
                              $"Подробности: {(!string.IsNullOrEmpty(errorContent) ? errorContent : "нет дополнительной информации")}\n\n" +
                              $"Попробуйте повторить попытку позже или обратитесь к администратору.";
        _messageBoxService.ShowError(errorMessage, "Ошибка сервера");
    }

    private void HandleHttpRequestErrorAsync(HttpRequestException ex)
    {
        string errorMessage = $"Не удалось отправить данные на сервер.\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Ошибка сети");
    }

    private void HandleTimeoutErrorAsync(TaskCanceledException ex)
    {
        string errorMessage = $"Превышено время ожидания ответа от сервера.\n" +
                              $"Попробуйте повторить попытку позже или проверьте стабильность соединения.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowWarning(errorMessage, "Таймаут запроса");
    }

    private void HandleJsonErrorAsync(JsonException ex)
    {
        string errorMessage = $"Ошибка при обработке данных паспорта.\n" +
                              $"Проверьте корректность введённых данных и повторите попытку.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Ошибка данных");
    }

    private void HandleUnexpectedErrorAsync(Exception ex)
    {
        string errorMessage = $"Произошла неожиданная ошибка при отправке данных.\n" +
                              $"Попробуйте повторить попытку позже или обратитесь к администратору.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Неизвестная ошибка");
    }
}
