using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Model;
using PassRegulaParser.Ui;

namespace PassRegulaParser.Core.Handlers;

public class NewRecognitionHandler(string dirPath)
{
    readonly string _dirPath = dirPath;

    public void OnChangeDetected(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
        Thread.Sleep(1000);
        ProcessNewRecognition();
    }

    private void ProcessNewRecognition()
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

        Thread uiThread = new(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DocumentEditWindow(passportData));
        });
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
    }
}