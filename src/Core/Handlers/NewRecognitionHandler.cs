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

        PassportData passportData = new();

        try
        {
            DoctypeDataParserNode doctypeDataParserNode = new(doctypeDataFilepath);
            RussianPassportParserNode russianPassportParserNode = new(textDataFilepath);
            PhotoImageNode photoImageNode = new(photoFilepath);

            passportData = doctypeDataParserNode.Process(passportData);
            passportData = russianPassportParserNode.Process(passportData);
            passportData = photoImageNode.Process(passportData);
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