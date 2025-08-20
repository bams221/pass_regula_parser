using PassRegulaParser.Models;
using PassRegulaParser.Ui;

namespace PassRegulaParser.Core.Managers;


public class DocumentUiManager
{
    private DocumentEditWindow? _currentWindow;

    public void ShowDocumentEditWindow(PassportData passportData)
    {
        CloseCurrentWindow();

        Thread uiThread = new(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _currentWindow = new DocumentEditWindow(passportData);
            Application.Run(_currentWindow);
        });
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
    }

    public void CloseCurrentWindow()
    {
        if (_currentWindow != null)
        {
            _currentWindow.Invoke(() => _currentWindow.Close());
            _currentWindow.Dispose();
            _currentWindow = null;
        }
    }

    public bool IsWindowOpen => _currentWindow != null && !_currentWindow.IsDisposed;
}