namespace PassRegulaParser.Core.Handlers;

public class FileChangeHandler
{
    public void OnChangeDetected(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
    }
}