using PassRegulaParser.Core.Interfaces;

namespace PassRegulaParser.Ui.Services;

public class MessageBoxService : IMessageBoxService
{
    public void ShowError(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public void ShowWarning(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public void ShowInfo(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}