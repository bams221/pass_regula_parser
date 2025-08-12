namespace PassRegulaParser.Core.Handlers;

public class NewRecognitionHandler
{
    public void OnNewRecognition(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
    }
}