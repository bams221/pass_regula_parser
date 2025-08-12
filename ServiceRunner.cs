using PassRegulaParser.Core.DirWatcher;
using PassRegulaParser.Core.Handlers;
namespace PassRegulaParser;

class ServiceRunner
{
    private const string PathToWatch = "C:\\RD\\last_res";
    private const string FileFilter = "Doctype_Data.json";

    public static void Main(string[] args)
    {
        var changeHandler = new NewRecognitionHandler();
        var watcher = new DirWatcher(
            PathToWatch,
            FileFilter,
            changeHandler.OnNewRecognition);

        Console.WriteLine("Watcher service started!");

        watcher.Start();
    }
}