using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.Managers;
using PassRegulaParser.Core.Nodes;
using PassRegulaParser.Models;

namespace PassRegulaParser.Core.Handlers;

public class DocumentRecognitionCoordinator(string dirPath)
{
    readonly string _dirPath = dirPath;
    private readonly DocumentUiManager _uiManager = new();

    public void OnChangeDetected(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
        Thread.Sleep(1000);
        ProcessRecognition();
    }

    private void ProcessRecognition()
    {
        try
        {
            PassportData passportData = CollectPassportData();

            _uiManager.ShowDocumentEditWindow(passportData);
        }
        catch (ParsingException ex)
        {
            Console.WriteLine("Parsign error: " + ex.Message);
        }
    }

    private PassportData CollectPassportData()
    {
        PassportData passportData = new();

        string doctypeDataFilepath = Path.Combine(_dirPath, "ChoosenDoctype_Data.json");
        DoctypeDataParserNode doctypeDataParserNode = new(doctypeDataFilepath);

        string textDataFilepath = Path.Combine(_dirPath, "Text_Data.json");
        RussianPassportParserNode rusPasspParserNode = new(textDataFilepath);

        string photoFilepath = Path.Combine(_dirPath, "Photo.jpg");
        PhotoImageNode photoImageNode = new(photoFilepath);

        CorrectorNode correctorNode = new();

        passportData = doctypeDataParserNode.Process(passportData);
        passportData = rusPasspParserNode.Process(passportData);
        passportData = photoImageNode.Process(passportData);
        passportData = correctorNode.Process(passportData);

        Console.WriteLine("Passport data recognized!");
        return passportData;
    }
}