using PassRegulaParser.Models;
using PassRegulaParser.Ui;

namespace PassRegulaParser.Core.Managers;

public class DocumentUiManager : IDisposable
{
    private DocumentEditWindow? _currentWindow;
    private Thread? _uiThread;

    public void ShowDocumentEditWindow(PassportData passportData)
    {
        ClosePrevWindow();

        _uiThread = new Thread(() =>
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                _currentWindow = new DocumentEditWindow(passportData);
                Application.Run(_currentWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UI Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _currentWindow?.Dispose();
                _currentWindow = null;
            }
        });
        _uiThread.SetApartmentState(ApartmentState.STA);
        _uiThread.Start();
    }

    public void ClosePrevWindow()
    {
        if (_currentWindow == null) return;

        // Use BeginInvoke to avoid blocking
        _currentWindow.BeginInvoke((MethodInvoker)delegate
        {
            _currentWindow?.Close();
        });

        if (_uiThread != null && _uiThread.IsAlive)
        {
            _uiThread.Join(TimeSpan.FromSeconds(2));
        }

        _currentWindow?.Dispose();
        _currentWindow = null;
        _uiThread = null;
    }

    public bool IsWindowOpen => _currentWindow != null && !_currentWindow.IsDisposed;

    public void Dispose()
    {
        ClosePrevWindow();
        GC.SuppressFinalize(this);
    }
}
