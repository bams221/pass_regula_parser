namespace PassRegulaParser.Core.Interfaces;

public interface IMessageBoxService
{
    void ShowError(string message, string title);
    void ShowWarning(string message, string title);
    void ShowInfo(string message, string title);
}