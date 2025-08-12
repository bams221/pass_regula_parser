class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Directory Watcher started!");
        Console.WriteLine("Press 'q' to quit.");

        string pathToWatch = "C:\\RD\\last_res";

        using FileSystemWatcher watcher = new();
        watcher.Path = pathToWatch;

        watcher.NotifyFilter = NotifyFilters.LastWrite
                             | NotifyFilters.FileName
                             | NotifyFilters.DirectoryName;

        // Включаем отслеживание всех файлов
        watcher.Filter = "Doctype_Data.json";

        watcher.Changed += OnNewRecognition;

        watcher.EnableRaisingEvents = true;

        // Ждем нажатия 'q' для выхода
        while (Console.Read() != 'q') ;
    }

    private static void OnNewRecognition(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"Changes detected in: {e.FullPath} ({e.ChangeType})");
    }

}