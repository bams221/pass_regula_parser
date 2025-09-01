using System.Net;
using System.Text.Json;
using PassRegulaParser.Core.Interfaces;

namespace PassRegulaParser.Api;

public class ApiErrorHandler(IMessageBoxService messageBoxService)
{
    private readonly IMessageBoxService _messageBoxService = messageBoxService;

    public bool HandleError(
        HttpResponseMessage response,
        string errorMessage,
        Exception? exception = null)
    {
        if (response.IsSuccessStatusCode)
            return true;

        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            HandleValidationError(errorMessage);
        }
        else
        {
            HandleServerError(response.StatusCode, errorMessage);
        }

        return false;
    }

    public void HandleException(Exception ex)
    {
        switch (ex)
        {
            case HttpRequestException httpEx:
                HandleHttpRequestError(httpEx);
                break;
            case TaskCanceledException timeoutEx:
                HandleTimeoutError(timeoutEx);
                break;
            case JsonException jsonEx:
                HandleJsonError(jsonEx);
                break;
            default:
                HandleUnexpectedError(ex);
                break;
        }
    }

    private void HandleValidationError(string errorMessage)
    {
        try
        {
            using JsonDocument doc = JsonDocument.Parse(errorMessage);
            var errors = doc.RootElement.GetProperty("detail").EnumerateArray();
            var errorMessages = new List<string>();
            foreach (var error in errors)
            {
                string msg = error.GetProperty("msg").GetString() ?? "";
                errorMessages.Add(msg);
            }
            string errorMessageExplained = $"Ошибка валидации данных паспорта:\n\n" +
                                  $"{string.Join("\n", errorMessages)}\n\n" +
                                  $"Проверьте корректность введённых данных и повторите попытку.";
            _messageBoxService.ShowError(errorMessageExplained, "Ошибка валидации данных");
        }
        catch (Exception)
        {
            string errorMessageExplained = $"Ошибка валидации данных паспорта.\n" +
                                  $"Проверьте корректность введённых данных и повторите попытку.\n\n" +
                                  $"Детали ошибки: {(!string.IsNullOrEmpty(errorMessage) ? errorMessage : "нет дополнительной информации")}";
            _messageBoxService.ShowError(errorMessageExplained, "Ошибка валидации данных");
        }
    }

    private void HandleServerError(HttpStatusCode statusCode, string errorMessage)
    {
        string errorMessageExplained = $"Сервер вернул ошибку: {statusCode}\n" +
                              $"Подробности: {(!string.IsNullOrEmpty(errorMessage) ? errorMessage : "нет дополнительной информации")}\n\n" +
                              $"Попробуйте повторить попытку позже или обратитесь к администратору.";
        _messageBoxService.ShowError(errorMessageExplained, "Ошибка сервера");
    }

    private void HandleHttpRequestError(HttpRequestException ex)
    {
        string errorMessage = $"Не удалось отправить данные на сервер.\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Ошибка сети");
    }

    private void HandleTimeoutError(TaskCanceledException ex)
    {
        string errorMessage = $"Превышено время ожидания ответа от сервера.\n" +
                              $"Попробуйте повторить попытку позже или проверьте стабильность соединения.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowWarning(errorMessage, "Таймаут запроса");
    }

    private void HandleJsonError(JsonException ex)
    {
        string errorMessage = $"Ошибка при обработке данных паспорта.\n" +
                              $"Проверьте корректность введённых данных и повторите попытку.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Ошибка данных");
    }

    private void HandleUnexpectedError(Exception ex)
    {
        string errorMessage = $"Произошла неожиданная ошибка при отправке данных.\n" +
                              $"Попробуйте повторить попытку позже или обратитесь к администратору.\n\n" +
                              $"Детали ошибки: {ex.Message}";
        _messageBoxService.ShowError(errorMessage, "Неизвестная ошибка");
    }
}
