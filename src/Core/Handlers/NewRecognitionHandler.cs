using PassRegulaParser.Core.Dto;
using PassRegulaParser.Core.Exceptions;
using PassRegulaParser.Core.ParserNodes;

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

        PassportData passportData = new();

        try
        {
            DoctypeDataParserNode doctypeDataParserNode = new(doctypeDataFilepath);

            passportData = doctypeDataParserNode.Process(passportData);
        }
        catch (ParsingException ex)
        {
            Console.WriteLine("Parsign error: " + ex.Message);
            return;
        }

        Console.WriteLine("Recognized passport Data: " + passportData);
    }
}