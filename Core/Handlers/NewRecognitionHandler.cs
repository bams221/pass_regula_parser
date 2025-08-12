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
        string filePath = Path.Combine(_dirPath, "Doctype_Data.json");
        // TODO: instead of printing to console, parse json and get document data field.
        //       Also move this funtion to separate method 
        try
        {
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                Console.WriteLine("Content of Doctype_Data.json:");
                Console.WriteLine(jsonContent);
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }
}