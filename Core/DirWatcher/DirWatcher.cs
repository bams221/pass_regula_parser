namespace PassRegulaParser.Core.DirWatcher;

public class DirWatcher
{
    public FileSystemWatcher Watcher;

    public DirWatcher(string path, string changeFilter, FileSystemEventHandler onChangeHandler)
    {
        Watcher = new FileSystemWatcher
        {
            Path = path,
            Filter = changeFilter,
            NotifyFilter = NotifyFilters.LastWrite
                        | NotifyFilters.FileName
                        | NotifyFilters.DirectoryName
        };

        Watcher.Changed += onChangeHandler;
    }

    public void Start()
    {
        Watcher.EnableRaisingEvents = true;
        while (Console.Read() != 'q') ;
    }
}