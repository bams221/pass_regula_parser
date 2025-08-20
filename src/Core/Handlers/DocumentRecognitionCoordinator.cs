using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Models;
using PassRegulaParser.Ui;

namespace PassRegulaParser.Core.Handlers;

public class DocumentRecognitionCoordinator(string dirPath)
{
    readonly string _dirPath = dirPath;
    private DocumentEditWindow? _currentWindow;

    public void OnChangeDetected(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
        Thread.Sleep(1000);
        ProcessRecognition();
    }

    private void ProcessRecognition()
    {
        string doctypeDataFilepath = Path.Combine(_dirPath, "ChoosenDoctype_Data.json");
        string textDataFilepath = Path.Combine(_dirPath, "Text_Data.json");
        string photoFilepath = Path.Combine(_dirPath, "Photo.jpg");
        string secondPageFolderPath = Path.Combine(_dirPath, "Page1");

        PassportData passportData = new();

        try
        {
            DoctypeDataParserNode doctypeDataParserNode = new(doctypeDataFilepath);
            RussianPassportParserNode rusPasspParserNode = new(textDataFilepath);
            RussianPassportSecondPageParserNode rusPasspSecondPageParserNode = new(secondPageFolderPath);
            PhotoImageNode photoImageNode = new(photoFilepath);

            passportData = doctypeDataParserNode.Process(passportData);
            passportData = rusPasspParserNode.Process(passportData);
            passportData = photoImageNode.Process(passportData);
            passportData = rusPasspSecondPageParserNode.Process(passportData);
        }
        catch (ParsingException ex)
        {
            Console.WriteLine("Parsign error: " + ex.Message);
            return;
        }

        Console.WriteLine("Recognized passport Data: " + passportData);

        if (_currentWindow != null)
        {
            _currentWindow.Invoke(() => _currentWindow.Close());
            _currentWindow.Dispose();
        }

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
}
