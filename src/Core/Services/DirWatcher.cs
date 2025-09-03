namespace PassRegulaParser.Core.Services;

public class DirWatcher : IDisposable
{
    public FileSystemWatcher Watcher;
    private readonly Dictionary<string, DateTime> _lastEventTime = [];
    private readonly object _lock = new();

    public DirWatcher(
        string path,
        string changeFilter,
        FileSystemEventHandler onChangeHandler
        )
    {
        Watcher = new FileSystemWatcher
        {
            Path = path,
            Filter = changeFilter,
            NotifyFilter = NotifyFilters.LastWrite
                        | NotifyFilters.FileName
                        | NotifyFilters.DirectoryName,

            InternalBufferSize = 65536,
            IncludeSubdirectories = false
        };

        Watcher.Changed += (sender, e) => DebouncedOnChange(sender, e, onChangeHandler);
    }

    private void DebouncedOnChange(object sender, FileSystemEventArgs e, FileSystemEventHandler handler)
    {
        lock (_lock)
        {
            if (_lastEventTime.TryGetValue(e.FullPath, out var lastTime) &&
                (DateTime.Now - lastTime).TotalMilliseconds < 500)
            {
                return;
            }

            _lastEventTime[e.FullPath] = DateTime.Now;
        }

        handler(sender, e);
    }

    public void Start()
    {
        Watcher.EnableRaisingEvents = true;
        try
        {
           Task.Delay(Timeout.Infinite).Wait();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Task cancelled.");
        }
    }

    public void Stop()
    {
        Watcher.EnableRaisingEvents = false;
    }

    public void Dispose()
    {
        Stop();
        Watcher?.Dispose();
        GC.SuppressFinalize(this);
    }
}