using PassRegulaParser.Core.Services;
using PassRegulaParser.Core.Handlers;
using PassRegulaParser.Config;

namespace PassRegulaParser;

class MainRunner
{
    private static TrayIconService? _trayIconService;
    private static DirWatcher? _watcher;

    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            string pathToWatch = AppConfiguration.Configuration["PathToWatch"]
                ?? throw new FormatException("PathToWatch is missing in appsettings.json config file");
            string fileFilter = AppConfiguration.Configuration["FileFilter"]
                ?? throw new FormatException("FileFilter is missing in appsettings.json config file");

            DocumentRecognitionCoordinator newRecognitionHandler = new(pathToWatch);
            _watcher = new DirWatcher(pathToWatch, fileFilter, newRecognitionHandler.OnChangeDetected);

            var trayThread = new Thread(() =>
                {
                    _trayIconService = new TrayIconService(
                        appName: "Regula Passport Monitor",
                        onDoubleClick: () => Task.Run(() => newRecognitionHandler.ProcessRecognition())
                    );
                    Application.Run();
                }
            );

            trayThread.SetApartmentState(ApartmentState.STA);
            trayThread.IsBackground = true;
            trayThread.Start();

            _watcher.Start();

            Application.Run();

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка запуска приложения: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _trayIconService?.Dispose();
            _watcher?.Dispose();
        }
    }
}