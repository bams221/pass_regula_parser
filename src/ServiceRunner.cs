using PassRegulaParser.Core.DirWatcher;
using PassRegulaParser.Core.Handlers;
namespace PassRegulaParser;

class ServiceRunner
{
    private const string PathToWatch = "C:\\RD\\last_res";
    private const string FileFilter = "ChoosenDoctype_Data.json";

    public static void Main(string[] args)
    {
        var newRecognitionHandler = new NewRecognitionHandler(PathToWatch);
        var watcher = new DirWatcher(
            PathToWatch,
            FileFilter,
            newRecognitionHandler.OnChangeDetected);

        Console.WriteLine("Watcher service started!");

        watcher.Start();
    }
}