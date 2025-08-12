namespace PassRegulaParser.Core.Handlers;

public class NewRecognitionHandler
{
    public void OnNewRecognition(object source, FileSystemEventArgs e)
    {
        Thread.Sleep(1000);
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
    }
}