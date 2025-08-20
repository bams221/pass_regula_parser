using PassRegulaParser.Core.Services;
using PassRegulaParser.Core.Handlers;

namespace PassRegulaParser;

class ServiceRunner
{
    private const string PathToWatch = "C:\\RD\\last_res";
    private const string FileFilter = "ChoosenDoctype_Data.json";

    public static void Main(string[] args)
    {
        NewRecognitionHandler newRecognitionHandler = new(PathToWatch);
        DirWatcherService watcher = new(
            PathToWatch,
            FileFilter,
            newRecognitionHandler.OnChangeDetected);

        Console.WriteLine("Watcher service started!");

        watcher.Start();
    }
}